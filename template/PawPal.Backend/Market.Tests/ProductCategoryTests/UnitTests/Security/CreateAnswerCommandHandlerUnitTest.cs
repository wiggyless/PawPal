using Microsoft.EntityFrameworkCore;
using PawPal.Application.Common.Exceptions;
using PawPal.Application.Modules.Security.Answers.Commands.Create;
using PawPal.Domain.Entities.Identity;
using PawPal.Domain.Entities.Security;
using System.Security.Cryptography;
using System.Text;
using Xunit;

public class CreateAnswerCommandHandlerUnitTest
{
    private readonly DatabaseContext _context;
    private readonly CreateAnswerCommandHandler _sut;

    public CreateAnswerCommandHandlerUnitTest()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new DatabaseContext(options, TimeProvider.System);
        _sut = new CreateAnswerCommandHandler(_context);
    }

    private async Task<UserEntity> SeedUserAsync(string email = "alice@example.com")
    {
        var user = new UserEntity { Id = 1, Username = "Alice", FcmToken = "tokenA", CityId = 10, Email = email };
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
    public async Task Handle_ShouldCreateAllThreeAnswers_WhenRequestIsValid()
    {
        var user = await SeedUserAsync();
        var questions = await SeedQuestionsAsync(3);

        var command = new CreateAnswerCommand
        {
            Email = user.Email,
            Answers = new Dictionary<int, string>
            {
                { questions[0].Id, "Fluffy" },
                { questions[1].Id, "Sarajevo" },
                { questions[2].Id, "Blue" }
            }
        };

        var result = await _sut.Handle(command, CancellationToken.None);

        Assert.True(result > 0);

        var saved = await _context.SecurityAnswers
            .Where(a => a.Email == user.Email)
            .ToListAsync();

        Assert.Equal(3, saved.Count);
        Assert.All(saved, a => Assert.Equal(user.Email, a.Email));
    }

    [Fact]
    public async Task Handle_ShouldMapCorrectQuestionIdToEachAnswer()
    {
        var user = await SeedUserAsync();
        var questions = await SeedQuestionsAsync(3);

        var command = new CreateAnswerCommand
        {
            Email = user.Email,
            Answers = new Dictionary<int, string>
            {
                { questions[0].Id, "Fluffy" },
                { questions[1].Id, "Sarajevo" },
                { questions[2].Id, "Blue" }
            }
        };

        await _sut.Handle(command, CancellationToken.None);

        var saved = await _context.SecurityAnswers
            .Where(a => a.Email == user.Email)
            .ToListAsync();

        Assert.Contains(saved, a => a.QuestionID == questions[0].Id);
        Assert.Contains(saved, a => a.QuestionID == questions[1].Id);
        Assert.Contains(saved, a => a.QuestionID == questions[2].Id);
    }

    [Fact]
    public async Task Handle_ShouldHashAnswer_UsingSHA256()
    {
        var user = await SeedUserAsync();
        var questions = await SeedQuestionsAsync(1);

        var command = new CreateAnswerCommand
        {
            Email = user.Email,
            Answers = new Dictionary<int, string> { { questions[0].Id, "Fluffy" } }
        };

        await _sut.Handle(command, CancellationToken.None);

        var saved = await _context.SecurityAnswers.FirstOrDefaultAsync(a => a.Email == user.Email);

        Assert.NotNull(saved);
        Assert.Equal(Hash("Fluffy"), saved!.Answer);
        Assert.NotEqual("Fluffy", saved.Answer);
    }

    [Fact]
    public async Task Handle_ShouldThrowConflict_WhenAQuestionDoesNotExist()
    {
        var user = await SeedUserAsync();
        var questions = await SeedQuestionsAsync(2);

        var command = new CreateAnswerCommand
        {
            Email = user.Email,
            Answers = new Dictionary<int, string>
            {
                { questions[0].Id, "Fluffy" },
                { 999, "Nonexistent" }
            }
        };

        await Assert.ThrowsAsync<PawPalConflictException>(
            () => _sut.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrowConflict_WhenEmailIsEmpty()
    {
        await SeedUserAsync();
        var questions = await SeedQuestionsAsync(1);

        var command = new CreateAnswerCommand
        {
            Email = "   ",
            Answers = new Dictionary<int, string> { { questions[0].Id, "Fluffy" } }
        };

        await Assert.ThrowsAsync<PawPalConflictException>(
            () => _sut.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFound_WhenEmailDoesNotExist()
    {
        await SeedUserAsync("alice@example.com");
        var questions = await SeedQuestionsAsync(1);

        var command = new CreateAnswerCommand
        {
            Email = "unknown@example.com",
            Answers = new Dictionary<int, string> { { questions[0].Id, "Fluffy" } }
        };

        await Assert.ThrowsAsync<PawPalNotFoundException>(
            () => _sut.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrowConflict_WhenAnAnswerIsEmpty()
    {
        var user = await SeedUserAsync();
        var questions = await SeedQuestionsAsync(2);

        var command = new CreateAnswerCommand
        {
            Email = user.Email,
            Answers = new Dictionary<int, string>
            {
                { questions[0].Id, "Fluffy" },
                { questions[1].Id, "   " }
            }
        };

        await Assert.ThrowsAsync<PawPalConflictException>(
            () => _sut.Handle(command, CancellationToken.None));
    }
}