using Microsoft.EntityFrameworkCore;
using Moq;
using PawPal.Application.Abstractions;
using PawPal.Application.Common.Exceptions;
using PawPal.Application.Modules.Comments.Commands.Create;
using PawPal.Domain.Common;
using PawPal.Domain.Entities.Identity;
using PawPal.Domain.Entities.Posts;
using Xunit;

namespace PawPal.Tests.UnitTests.Comments
{
    // Regression coverage for: CreateCommentCommand used to accept a client-supplied UserID with
    // no check against the caller at all — anyone authenticated could comment as anyone else.
    // The command no longer carries a UserID; the author is always IAppCurrentUser.
    public class CreateCommentCommandHandlerUnitTest
    {
        private readonly DatabaseContext _context;
        private readonly Mock<IAppCurrentUser> _currentUserMock;
        private readonly Mock<ICommentHubService> _hubMock;
        private readonly CreateCommentCommandHandler _sut;

        public CreateCommentCommandHandlerUnitTest()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new DatabaseContext(options, TimeProvider.System);
            _currentUserMock = new Mock<IAppCurrentUser>();
            _hubMock = new Mock<ICommentHubService>();
            _sut = new CreateCommentCommandHandler(_context, _currentUserMock.Object, _hubMock.Object);
        }

        private async Task<UserEntity> SeedUserAsync(int id)
        {
            var user = new UserEntity { Id = id, Username = $"user{id}", Email = $"user{id}@example.com" };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        private async Task<PostsEntity> SeedPostAsync(int id, int ownerId)
        {
            var post = new PostsEntity { Id = id, UserId = ownerId, AnimalID = 1, DateAdded = DateTime.UtcNow };
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return post;
        }

        [Fact]
        public async Task Handle_ShouldAttributeCommentToTheAuthenticatedCaller()
        {
            var author = await SeedUserAsync(1);
            var post = await SeedPostAsync(10, ownerId: 2);
            _currentUserMock.Setup(x => x.UserId).Returns(author.Id);

            var id = await _sut.Handle(new CreateCommentCommand { PostID = post.Id, Content = "Nice post!" }, CancellationToken.None);

            var saved = await _context.Comments.FindAsync(id);
            Assert.NotNull(saved);
            Assert.Equal(author.Id, saved!.UserId);
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenCallerIsAnonymous()
        {
            await SeedUserAsync(1);
            var post = await SeedPostAsync(10, ownerId: 2);
            _currentUserMock.Setup(x => x.UserId).Returns((int?)null);

            await Assert.ThrowsAsync<PawPalConflictException>(
                () => _sut.Handle(new CreateCommentCommand { PostID = post.Id, Content = "Hi" }, CancellationToken.None));

            var count = await _context.Comments.CountAsync();
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFound_WhenPostDoesNotExist()
        {
            var author = await SeedUserAsync(1);
            _currentUserMock.Setup(x => x.UserId).Returns(author.Id);

            await Assert.ThrowsAsync<PawPalNotFoundException>(
                () => _sut.Handle(new CreateCommentCommand { PostID = 999, Content = "Hi" }, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenContentIsEmpty()
        {
            var author = await SeedUserAsync(1);
            var post = await SeedPostAsync(10, ownerId: 2);
            _currentUserMock.Setup(x => x.UserId).Returns(author.Id);

            await Assert.ThrowsAsync<PawPalConflictException>(
                () => _sut.Handle(new CreateCommentCommand { PostID = post.Id, Content = "" }, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldNotifyHub_WithTheAuthenticatedCallerAsAuthor()
        {
            var author = await SeedUserAsync(1);
            var post = await SeedPostAsync(10, ownerId: 2);
            _currentUserMock.Setup(x => x.UserId).Returns(author.Id);

            await _sut.Handle(new CreateCommentCommand { PostID = post.Id, Content = "Hi" }, CancellationToken.None);

            _hubMock.Verify(h => h.SendCommentNotification(
                It.Is<CommentDto>(dto => dto.UserID == author.Id)), Times.Once);
        }
    }
}
