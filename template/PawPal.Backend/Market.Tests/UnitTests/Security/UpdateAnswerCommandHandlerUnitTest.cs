using Microsoft.EntityFrameworkCore;
using Moq;
using PawPal.Application.Abstractions;
using PawPal.Application.Common.Exceptions;
using PawPal.Application.Modules.Security.Answers.Commands.Update;
using PawPal.Domain.Entities.Identity;
using PawPal.Domain.Entities.Security;
using System.Security.Cryptography;
using System.Text;
using Xunit;

public class UpdateAnswerCommandHandlerUnitTest
{
    private readonly DatabaseContext _context;
    private readonly Mock<IAppCurrentUser> _currentUserMock;
    private readonly UpdateAnswerCommandHandler _sut;

    public UpdateAnswerCommandHandlerUnitTest()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new DatabaseContext(options, TimeProvider.System);
        _currentUserMock = new Mock<IAppCurrentUser>();
        _sut = new UpdateAnswerCommandHandler(_currentUserMock.Object, _context);
    }

    private async Task<UserEntity> SeedUserAsync(int id, string email)
    {
        var user = new UserEntity { Id = id, Username = email, Email = email };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    private async Task<List<SecurityQuestion>> SeedQuestionsAsync(int count = 3)
    {
        var questions = new List<SecurityQuestion>();
        for (int i = 1; i <= count; i++)
        {
            questions.Add(new SecurityQuestion { Id = i, Question = $"Question {i}" });
        }
        _context.SecurityQuestions.AddRange(questions);
        await _context.SaveChangesAsync();
        return questions;
    }

    private static string Hash(string input)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes);
    }

    [Fact]
    public async Task Handle_ShouldPersistNewAnswerHash_ForExistingAnswer()
    {
        var user = await SeedUserAsync(1, "alice@example.com");
        _currentUserMock.Setup(x => x.UserId).Returns(user.Id);
        var questions = await SeedQuestionsAsync(1);

        _context.SecurityAnswers.Add(new SecurityAnswers
        {
            UserId = user.Id,
            QuestionID = questions[0].Id,
            Answer = Hash("OldAnswer"),
        });
        await _context.SaveChangesAsync();

        var command = new UpdateAnswerCommand
        {
            Answers = new Dictionary<int, string> { { questions[0].Id, "NewAnswer" } },
        };

        await _sut.Handle(command, CancellationToken.None);

        var saved = await _context.SecurityAnswers
            .FirstAsync(a => a.UserId == user.Id && a.QuestionID == questions[0].Id);

        Assert.Equal(Hash("NewAnswer"), saved.Answer);
        Assert.NotEqual(Hash("OldAnswer"), saved.Answer);
    }

    [Fact]
    public async Task Handle_ShouldCreateAnswer_WhenQuestionNotPreviouslyAnswered()
    {
        var user = await SeedUserAsync(1, "alice@example.com");
        _currentUserMock.Setup(x => x.UserId).Returns(user.Id);
        var questions = await SeedQuestionsAsync(1);

        var command = new UpdateAnswerCommand
        {
            Answers = new Dictionary<int, string> { { questions[0].Id, "BrandNewAnswer" } },
        };

        await _sut.Handle(command, CancellationToken.None);

        var saved = await _context.SecurityAnswers
            .Where(a => a.UserId == user.Id && a.QuestionID == questions[0].Id)
            .ToListAsync();

        Assert.Single(saved);
        Assert.Equal(Hash("BrandNewAnswer"), saved[0].Answer);
    }

    [Fact]
    public async Task Handle_ShouldNotModifyAnotherUsersAnswer_ForTheSameQuestion()
    {
        var owner = await SeedUserAsync(1, "alice@example.com");
        var attacker = await SeedUserAsync(2, "mallory@example.com");
        _currentUserMock.Setup(x => x.UserId).Returns(attacker.Id);
        var questions = await SeedQuestionsAsync(1);

        _context.SecurityAnswers.Add(new SecurityAnswers
        {
            UserId = owner.Id,
            QuestionID = questions[0].Id,
            Answer = Hash("OwnersAnswer"),
        });
        await _context.SaveChangesAsync();

        var command = new UpdateAnswerCommand
        {
            Answers = new Dictionary<int, string> { { questions[0].Id, "AttackersAnswer" } },
        };

        await _sut.Handle(command, CancellationToken.None);

        var ownersAnswer = await _context.SecurityAnswers
            .FirstAsync(a => a.UserId == owner.Id && a.QuestionID == questions[0].Id);
        Assert.Equal(Hash("OwnersAnswer"), ownersAnswer.Answer);

        var attackersAnswer = await _context.SecurityAnswers
            .SingleAsync(a => a.UserId == attacker.Id && a.QuestionID == questions[0].Id);
        Assert.Equal(Hash("AttackersAnswer"), attackersAnswer.Answer);
    }

    [Fact]
    public async Task Handle_ShouldThrowConflict_WhenUserIsNotAuthenticated()
    {
        await SeedUserAsync(1, "alice@example.com");
        _currentUserMock.Setup(x => x.UserId).Returns((int?)null);
        var questions = await SeedQuestionsAsync(1);

        var command = new UpdateAnswerCommand
        {
            Answers = new Dictionary<int, string> { { questions[0].Id, "SomeAnswer" } },
        };

        await Assert.ThrowsAsync<PawPalConflictException>(
            () => _sut.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrowConflict_WhenAQuestionDoesNotExist()
    {
        var user = await SeedUserAsync(1, "alice@example.com");
        _currentUserMock.Setup(x => x.UserId).Returns(user.Id);

        var command = new UpdateAnswerCommand
        {
            Answers = new Dictionary<int, string> { { 999, "SomeAnswer" } },
        };

        await Assert.ThrowsAsync<PawPalConflictException>(
            () => _sut.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrowConflict_WhenAnswerLengthIsInvalid()
    {
        var user = await SeedUserAsync(1, "alice@example.com");
        _currentUserMock.Setup(x => x.UserId).Returns(user.Id);
        var questions = await SeedQuestionsAsync(1);

        var command = new UpdateAnswerCommand
        {
            Answers = new Dictionary<int, string> { { questions[0].Id, "short" } },
        };

        await Assert.ThrowsAsync<PawPalConflictException>(
            () => _sut.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldSkipUnchanged_WhenAnswerIsEmpty()
    {
        var user = await SeedUserAsync(1, "alice@example.com");
        _currentUserMock.Setup(x => x.UserId).Returns(user.Id);
        var questions = await SeedQuestionsAsync(1);

        _context.SecurityAnswers.Add(new SecurityAnswers
        {
            UserId = user.Id,
            QuestionID = questions[0].Id,
            Answer = Hash("OriginalAnswer"),
        });
        await _context.SaveChangesAsync();

        var command = new UpdateAnswerCommand
        {
            Answers = new Dictionary<int, string> { { questions[0].Id, "   " } },
        };

        await _sut.Handle(command, CancellationToken.None);

        var saved = await _context.SecurityAnswers
            .FirstAsync(a => a.UserId == user.Id && a.QuestionID == questions[0].Id);
        Assert.Equal(Hash("OriginalAnswer"), saved.Answer);
    }
}
