using Moq;
using PawPal.Application.Abstractions;
using PawPal.Application.Common.Exceptions;
using PawPal.Application.Modules.Dashboard.Queries.GetSummary;
using PawPal.Domain.Entities.Adoptions;
using PawPal.Domain.Entities.Animal_Info;
using PawPal.Domain.Entities.Identity;
using PawPal.Domain.Entities.Moderation;
using PawPal.Domain.Entities.Posts;
using PawPal.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class GetDashboardSummaryQueryHandlerUnitTest
{
    private readonly DatabaseContext _context;
    private readonly Mock<IAppCurrentUser> _currentUserMock;
    private readonly GetDashboardSummaryQueryHandler _sut;

    public GetDashboardSummaryQueryHandlerUnitTest()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new DatabaseContext(options, TimeProvider.System);

        _currentUserMock = new Mock<IAppCurrentUser>();
        _sut = new GetDashboardSummaryQueryHandler(_context, _currentUserMock.Object);
    }

    private async Task SeedAsync()
    {
        var owner = new UserEntity { Id = 1, Username = "Owner" };
        var reporter = new UserEntity { Id = 2, Username = "Reporter" };
        _context.Users.AddRange(owner, reporter);

        var animal1 = new AnimalEntity { Id = 1, Name = "Rex", Breed = "Mixed" };
        var animal2 = new AnimalEntity { Id = 2, Name = "Fido", Breed = "Mixed" };
        _context.Animals.AddRange(animal1, animal2);

        var activePost = new PostsEntity { Id = 10, UserId = owner.Id, AnimalID = 1, Status = PostStatus.Active, DateAdded = DateTime.Now };
        var adoptedPost = new PostsEntity { Id = 11, UserId = owner.Id, AnimalID = 2, Status = PostStatus.Adopted, DateAdded = DateTime.Now };
        _context.Posts.AddRange(activePost, adoptedPost);

        _context.AdoptionRequests.AddRange(
            new AdoptionRequestEntity { Id = 100, UserId = reporter.Id, PostId = activePost.Id, RequirementId = 1, Status = AdoptionRequestStatus.Pending, DateSent = DateTime.Now },
            new AdoptionRequestEntity { Id = 101, UserId = reporter.Id, PostId = activePost.Id, RequirementId = 1, Status = AdoptionRequestStatus.Pending, DateSent = DateTime.Now },
            new AdoptionRequestEntity { Id = 102, UserId = reporter.Id, PostId = adoptedPost.Id, RequirementId = 1, Status = AdoptionRequestStatus.Accepted, DateSent = DateTime.Now });

        _context.ReportedPosts.Add(new ReportedPostsEntity { Id = 1, PostID = activePost.Id, UserID = reporter.Id, Reason = 0, DateSent = DateTime.Now });
        _context.ReportedUsers.Add(new ReportedUserEntity { Id = 1, ReportedUserID = owner.Id, ReportSentByUserID = reporter.Id, ReportUserEnum = 0, DateSent = DateTime.Now });

        await _context.SaveChangesAsync();
        _currentUserMock.Setup(c => c.RoleId).Returns(Roles.Admin);
    }

    [Fact]
    public async Task Handle_ReturnsCountsFromDatabase()
    {
        await SeedAsync();

        var result = await _sut.Handle(new GetDashboardSummaryQuery(), CancellationToken.None);

        Assert.Equal(1, result.ActiveListings);
        Assert.Equal(2, result.PendingAdoptionRequests);
        Assert.Equal(1, result.ReportedPosts);
        Assert.Equal(1, result.ReportedUsers);
        Assert.Equal(0, result.ReportedComments);
        Assert.Equal(0, result.ReportedProblems);
    }

    [Fact]
    public async Task Handle_WhenUserIsNotAdmin_ThrowsConflictException()
    {
        await SeedAsync();
        _currentUserMock.Setup(c => c.RoleId).Returns(Roles.BasicUser);

        await Assert.ThrowsAsync<PawPalConflictException>(
            () => _sut.Handle(new GetDashboardSummaryQuery(), CancellationToken.None));
    }
}
