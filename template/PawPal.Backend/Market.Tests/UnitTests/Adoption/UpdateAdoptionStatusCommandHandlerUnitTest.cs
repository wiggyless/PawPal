using Moq;
using PawPal.Application.Abstractions;
using PawPal.Application.Common.Exceptions;
using PawPal.Application.Modules.Adoptions.AdoptionRequests.Command.UpdateStatus;
using PawPal.Domain.Entities.Adoptions;
using PawPal.Domain.Entities.Animal_Info;
using PawPal.Domain.Entities.Identity;
using PawPal.Domain.Entities.Posts;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class UpdateAdoptionStatusCommandHandlerUnitTest
{
    private readonly DatabaseContext _context;
    private readonly Mock<IAppCurrentUser> _currentUserMock;
    private readonly Mock<IFirebaseNotificationService> _notificationMock;
    private readonly Mock<IEmailService> _emailMock;
    private readonly UpdateAdoptionStatusCommandHandler _sut;

    public UpdateAdoptionStatusCommandHandlerUnitTest()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new DatabaseContext(options, TimeProvider.System);

        _currentUserMock = new Mock<IAppCurrentUser>();
        _notificationMock = new Mock<IFirebaseNotificationService>();
        _emailMock = new Mock<IEmailService>();

        _sut = new UpdateAdoptionStatusCommandHandler(
            _context, _currentUserMock.Object, _notificationMock.Object, _emailMock.Object);
    }

    private async Task<(UserEntity owner, PostsEntity post, AdoptionRequestEntity request)> SeedPendingRequestAsync()
    {
        var owner = new UserEntity { Id = 1, Username = "Owner" };
        var requester = new UserEntity { Id = 2, Username = "Requester" };
        _context.Users.AddRange(owner, requester);

        var animal = new AnimalEntity { Id = 1, Name = "Rex", Breed = "Mixed" };
        _context.Animals.Add(animal);

        var post = new PostsEntity { Id = 10, UserId = owner.Id, AnimalID = 1, Status = PostStatus.Active, DateAdded = DateTime.Now };
        _context.Posts.Add(post);

        var request = new AdoptionRequestEntity
        {
            Id = 100,
            UserId = requester.Id,
            PostId = post.Id,
            RequirementId = 1,
            Status = AdoptionRequestStatus.Pending,
            DateSent = DateTime.Now,
        };
        _context.AdoptionRequests.Add(request);

        await _context.SaveChangesAsync();
        _currentUserMock.Setup(c => c.UserId).Returns(owner.Id);
        return (owner, post, request);
    }

    [Fact]
    public async Task Handle_WhenAccepted_SetsRequestAcceptedAndPostAdopted()
    {
        var (_, post, request) = await SeedPendingRequestAsync();

        await _sut.Handle(new UpdateAdoptionStatusCommand { Id = request.Id, Status = "Accepted" }, CancellationToken.None);

        var savedRequest = await _context.AdoptionRequests.FindAsync(request.Id);
        var savedPost = await _context.Posts.FindAsync(post.Id);
        Assert.Equal(AdoptionRequestStatus.Accepted, savedRequest!.Status);
        Assert.Equal(PostStatus.Adopted, savedPost!.Status);
    }

    [Fact]
    public async Task Handle_WhenAccepted_DeniesOtherPendingRequestsForSamePost()
    {
        var (_, post, request) = await SeedPendingRequestAsync();

        var otherRequester = new UserEntity { Id = 3, Username = "OtherRequester" };
        _context.Users.Add(otherRequester);
        var otherRequest = new AdoptionRequestEntity
        {
            Id = 101,
            UserId = otherRequester.Id,
            PostId = post.Id,
            RequirementId = 1,
            Status = AdoptionRequestStatus.Pending,
            DateSent = DateTime.Now,
        };
        _context.AdoptionRequests.Add(otherRequest);
        await _context.SaveChangesAsync();

        await _sut.Handle(new UpdateAdoptionStatusCommand { Id = request.Id, Status = "Accepted" }, CancellationToken.None);

        var savedOtherRequest = await _context.AdoptionRequests.FindAsync(otherRequest.Id);
        Assert.Equal(AdoptionRequestStatus.Denied, savedOtherRequest!.Status);
    }

    [Fact]
    public async Task Handle_WhenRequestAlreadyDecided_ThrowsConflictException()
    {
        var (_, _, request) = await SeedPendingRequestAsync();
        request.Status = AdoptionRequestStatus.Accepted;
        await _context.SaveChangesAsync();

        var ex = await Assert.ThrowsAsync<PawPalConflictException>(
            () => _sut.Handle(new UpdateAdoptionStatusCommand { Id = request.Id, Status = "Denied" }, CancellationToken.None));
        Assert.Contains("pending", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Handle_WithInvalidStatus_ThrowsConflictException()
    {
        var (_, _, request) = await SeedPendingRequestAsync();

        await Assert.ThrowsAsync<PawPalConflictException>(
            () => _sut.Handle(new UpdateAdoptionStatusCommand { Id = request.Id, Status = "Bogus" }, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WhenUserIsNotPostOwner_ThrowsConflictException()
    {
        var (_, _, request) = await SeedPendingRequestAsync();
        _currentUserMock.Setup(c => c.UserId).Returns(999);
        _currentUserMock.Setup(c => c.RoleId).Returns(0);

        await Assert.ThrowsAsync<PawPalConflictException>(
            () => _sut.Handle(new UpdateAdoptionStatusCommand { Id = request.Id, Status = "Accepted" }, CancellationToken.None));
    }
}
