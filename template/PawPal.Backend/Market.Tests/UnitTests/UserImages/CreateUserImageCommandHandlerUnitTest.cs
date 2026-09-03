using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using PawPal.Application.Abstractions;
using PawPal.Application.Common.Exceptions;
using PawPal.Application.Modules.UserImages.Commands.Create;
using PawPal.Domain.Entities.Identity;
using System.Text;
using Xunit;

namespace PawPal.Tests.UnitTests.UserImages
{
    // The command no longer carries a UserID — the owner is always taken from IAppCurrentUser,
    // so there is nothing left to compare against and no way to upload "as" another account.
    public class CreateUserImageCommandHandlerUnitTest
    {
        private readonly DatabaseContext _context;
        private readonly Mock<IAppCurrentUser> _currentUserMock;
        private readonly Mock<IFileStorageService> _fileStorageMock;
        private readonly CreateUserImageCommandHandler _sut;

        public CreateUserImageCommandHandlerUnitTest()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new DatabaseContext(options, TimeProvider.System);
            _currentUserMock = new Mock<IAppCurrentUser>();
            _fileStorageMock = new Mock<IFileStorageService>();
            _fileStorageMock
                .Setup(x => x.SaveFileAsync(It.IsAny<IFormFile>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("/users/User_1/generated.jpg");

            _sut = new CreateUserImageCommandHandler(_context, _currentUserMock.Object, _fileStorageMock.Object);
        }

        private async Task<UserEntity> SeedUserAsync(int id)
        {
            var user = new UserEntity { Id = id, Username = $"user{id}", Email = $"user{id}@example.com" };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        private static IFormFile MakeFile() =>
            new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("data")), 0, 4, "file", "photo.jpg");

        [Fact]
        public async Task Handle_ShouldSaveImage_ForTheAuthenticatedCaller()
        {
            var user = await SeedUserAsync(1);
            _currentUserMock.Setup(x => x.UserId).Returns(user.Id);

            var id = await _sut.Handle(new CreateUserImageCommand { Image = MakeFile() }, CancellationToken.None);

            Assert.True(id > 0);
            var saved = await _context.UserImage.FindAsync(id);
            Assert.Equal(user.Id, saved!.UserID);
            Assert.Equal("/users/User_1/generated.jpg", saved.PhotoURL);
        }

        [Fact]
        public async Task Handle_ShouldUseGeneratedSubFolder_ForTheCallersOwnId()
        {
            var user = await SeedUserAsync(1);
            _currentUserMock.Setup(x => x.UserId).Returns(user.Id);

            await _sut.Handle(new CreateUserImageCommand { Image = MakeFile() }, CancellationToken.None);

            _fileStorageMock.Verify(
                x => x.SaveFileAsync(It.IsAny<IFormFile>(), $"users/User_{user.Id}", It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenCallerIsAnonymous()
        {
            await SeedUserAsync(1);
            _currentUserMock.Setup(x => x.UserId).Returns((int?)null);

            await Assert.ThrowsAsync<PawPalConflictException>(
                () => _sut.Handle(new CreateUserImageCommand { Image = MakeFile() }, CancellationToken.None));

            _fileStorageMock.Verify(
                x => x.SaveFileAsync(It.IsAny<IFormFile>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFound_WhenCallerAccountDoesNotExist()
        {
            _currentUserMock.Setup(x => x.UserId).Returns(999);

            await Assert.ThrowsAsync<PawPalNotFoundException>(
                () => _sut.Handle(new CreateUserImageCommand { Image = MakeFile() }, CancellationToken.None));
        }
    }
}
