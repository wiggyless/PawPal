using Moq;
using PawPal.Application.Abstractions;
using PawPal.Application.Common.Exceptions;
using PawPal.Application.Modules.Adoptions.AdoptionRequests.Command.Create;
using PawPal.Application.Services;
using PawPal.Domain.Entities.Adoptions;
using PawPal.Domain.Entities.Identity;
using PawPal.Domain.Entities.Posts;

public class CreateAdoptionRequestCommandHandlerTests
{
    private readonly DatabaseContext _context;
    private readonly Mock<FirebaseNotificationService> _notificationMock;
    private readonly Mock<IAppCurrentUser> _currentUserMock;
    private readonly CreateAdoptionRequestCommandHandler _sut;

    public CreateAdoptionRequestCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new DatabaseContext(options,TimeProvider.System);

        _notificationMock = new Mock<FirebaseNotificationService>();
        _currentUserMock = new Mock<IAppCurrentUser>();

        _sut = new CreateAdoptionRequestCommandHandler(
            _context, _notificationMock.Object, _currentUserMock.Object);
    }

    private async Task<(UserEntity requester, UserEntity owner, PostsEntity post, AdoptionRequirementEntity req)> SeedAsync()
    {
        var requester = new UserEntity { Id = 1, Username = "Alice", FcmToken = "tokenA" };
        var owner = new UserEntity { Id = 2, Username = "Bob", FcmToken = "tokenB" };
        _context.Users.AddRange(requester, owner);

        var post = new PostsEntity { Id = 10, UserId = owner.Id, AnimalID = 1, Status = "Available", DateAdded = DateTime.Now };
        _context.Posts.Add(post);

        var req = new AdoptionRequirementEntity { Id = 100, HouseType = "Apartment" , PeopleAva="Always"};
        _context.AdoptionRequirements.Add(req);

        await _context.SaveChangesAsync();
        return (requester, owner, post, req);
    }

    private CreateAdoptionRequestCommand ValidCommand(int userId, int postId, int reqId) => new()
    {
        Status = "Pending",
        DateSend = DateTime.Now,
        UserID = userId,
        PostID = postId,
        RequirementID = reqId
    };

    [Fact]
    public async Task Handle_WithValidData_CreatesRequestAndReturnsId()
    {
        var (requester, owner, post, req) = await SeedAsync();
        _currentUserMock.Setup(c => c.IsAuthenticated).Returns(true);
        _currentUserMock.Setup(c => c.UserId).Returns(requester.Id);

        var command = ValidCommand(requester.Id, post.Id, req.Id);

        var id = await _sut.Handle(command, CancellationToken.None);

        var saved = await _context.AdoptionRequests.FindAsync(id);
        Assert.NotNull(saved);
        Assert.Equal("Pending", saved.Status);
        Assert.Equal(requester.Id, saved.UserId);
    }

    [Fact]
    public async Task Handle_WithNonExistentUser_ThrowsNotFoundException()
    {
        var (_, _, post, req) = await SeedAsync();
        var command = ValidCommand(userId: 999, post.Id, req.Id);

        await Assert.ThrowsAsync<PawPalNotFoundException>(
            () => _sut.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WhenNotAuthenticated_ThrowsConflictException()
    {
        var (requester, _, post, req) = await SeedAsync();
        _currentUserMock.Setup(c => c.IsAuthenticated).Returns(false);

        var command = ValidCommand(requester.Id, post.Id, req.Id);

        var ex = await Assert.ThrowsAsync<PawPalConflictException>(
            () => _sut.Handle(command, CancellationToken.None));
        Assert.Contains("not authenticated", ex.Message);
    }

    [Fact]
    public async Task Handle_WhenCurrentUserDoesNotMatchRequestUserId_ThrowsConflictException()
    {
        // Alice is authenticated but the command claims to be on behalf of a different user id
        var (requester, owner, post, req) = await SeedAsync();
        _currentUserMock.Setup(c => c.IsAuthenticated).Returns(true);
        _currentUserMock.Setup(c => c.UserId).Returns(owner.Id); // mismatched

        var command = ValidCommand(requester.Id, post.Id, req.Id);

        await Assert.ThrowsAsync<PawPalConflictException>(
            () => _sut.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WithNonExistentPost_ThrowsNotFoundException()
    {
        var (requester, _, _, req) = await SeedAsync();
        _currentUserMock.Setup(c => c.IsAuthenticated).Returns(true);
        _currentUserMock.Setup(c => c.UserId).Returns(requester.Id);

        var command = ValidCommand(requester.Id, postId: 999, req.Id);

        await Assert.ThrowsAsync<PawPalNotFoundException>(
            () => _sut.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WithNonExistentRequirement_ThrowsNotFoundException()
    {
        var (requester, _, post, _) = await SeedAsync();
        _currentUserMock.Setup(c => c.IsAuthenticated).Returns(true);
        _currentUserMock.Setup(c => c.UserId).Returns(requester.Id);

        var command = ValidCommand(requester.Id, post.Id, reqId: 999);

        await Assert.ThrowsAsync<PawPalNotFoundException>(
            () => _sut.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WhenUserRequestsOwnPost_ThrowsConflictException()
    {
        var (_, owner, post, req) = await SeedAsync();
        _currentUserMock.Setup(c => c.IsAuthenticated).Returns(true);
        _currentUserMock.Setup(c => c.UserId).Returns(owner.Id);

        var command = ValidCommand(owner.Id, post.Id, req.Id); // owner requesting their own post

        var ex = await Assert.ThrowsAsync<PawPalConflictException>(
            () => _sut.Handle(command, CancellationToken.None));
        Assert.Contains("own post", ex.Message);
    }

    [Fact]
    public async Task Handle_WhenPendingRequestAlreadyExists_ThrowsConflictException()
    {
        var (requester, _, post, req) = await SeedAsync();
        _currentUserMock.Setup(c => c.IsAuthenticated).Returns(true);
        _currentUserMock.Setup(c => c.UserId).Returns(requester.Id);

        _context.AdoptionRequests.Add(new AdoptionRequestEntity
        {
            UserId = requester.Id,
            PostId = post.Id,
            RequirementId = req.Id,
            Status = "Pending",
            DateSent = DateTime.Now
        });
        await _context.SaveChangesAsync();

        var command = ValidCommand(requester.Id, post.Id, req.Id);

        var ex = await Assert.ThrowsAsync<PawPalConflictException>(
            () => _sut.Handle(command, CancellationToken.None));
        Assert.Contains("already have a pending request", ex.Message);
    }

    [Fact]
    public async Task Handle_WhenPreviousRequestWasRejected_AllowsNewRequest()
    {
        // Confirms the duplicate check only blocks "Pending", not historical statuses
        var (requester, _, post, req) = await SeedAsync();
        _currentUserMock.Setup(c => c.IsAuthenticated).Returns(true);
        _currentUserMock.Setup(c => c.UserId).Returns(requester.Id);

        _context.AdoptionRequests.Add(new AdoptionRequestEntity
        {
            UserId = requester.Id,
            PostId = post.Id,
            RequirementId = req.Id,
            Status = "Rejected",
            DateSent = DateTime.Now.AddDays(-5)
        });
        await _context.SaveChangesAsync();

        var command = ValidCommand(requester.Id, post.Id, req.Id);

        var id = await _sut.Handle(command, CancellationToken.None);
        Assert.True(id > 0);
    }

    [Fact]
    public async Task Handle_WhenPostOwnerHasFcmToken_SendsNotification()
    {
        var (requester, owner, post, req) = await SeedAsync();
        _currentUserMock.Setup(c => c.IsAuthenticated).Returns(true);
        _currentUserMock.Setup(c => c.UserId).Returns(requester.Id);

        var command = ValidCommand(requester.Id, post.Id, req.Id);
        await _sut.Handle(command, CancellationToken.None);

        _notificationMock.Verify(n => n.SendAsync(
            owner.FcmToken,
            "New Adoption Request",
            It.Is<string>(msg => msg.Contains(requester.Username)),
            It.IsAny<string>()
        ), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenPostOwnerHasNoFcmToken_DoesNotSendNotification()
    {
        var (requester, owner, post, req) = await SeedAsync();
        owner.FcmToken = null;
        await _context.SaveChangesAsync();

        _currentUserMock.Setup(c => c.IsAuthenticated).Returns(true);
        _currentUserMock.Setup(c => c.UserId).Returns(requester.Id);

        var command = ValidCommand(requester.Id, post.Id, req.Id);
        await _sut.Handle(command, CancellationToken.None);

        _notificationMock.Verify(n => n.SendAsync(
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()
        ), Times.Never);
    }
}