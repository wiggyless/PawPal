using Microsoft.EntityFrameworkCore;
using Moq;
using PawPal.Application.Abstractions;
using PawPal.Application.Common.Exceptions;
using PawPal.Application.Modules.Users.Commands.UpdateRole;
using PawPal.Domain.Entities.Identity;
using Xunit;

namespace PawPal.Tests.UnitTests.User
{
    public class UpdateUserRoleCommandHandlerUnitTest
    {
        private readonly DatabaseContext _context;
        private readonly Mock<IAppCurrentUser> _currentUserMock;
        private readonly UpdateUserRoleCommandHandler _sut;

        public UpdateUserRoleCommandHandlerUnitTest()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new DatabaseContext(options, TimeProvider.System);
            _currentUserMock = new Mock<IAppCurrentUser>();
            _sut = new UpdateUserRoleCommandHandler(_context, _currentUserMock.Object);

            _context.Roles.AddRange(
                new RolesEntity { Id = 1, RoleName = "Basic user" },
                new RolesEntity { Id = 2, RoleName = "Verified user" },
                new RolesEntity { Id = 3, RoleName = "Admin" });
            _context.SaveChanges();
        }

        private async Task<UserEntity> SeedUserAsync(int id, int roleId)
        {
            var user = new UserEntity { Id = id, Username = $"user{id}", Email = $"user{id}@example.com", RoleId = roleId };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        [Fact]
        public async Task Handle_ShouldAssignRole_WhenCallerIsAdmin()
        {
            var user = await SeedUserAsync(1, roleId: 2);
            _currentUserMock.Setup(x => x.RoleId).Returns(3);

            await _sut.Handle(new UpdateUserRoleCommand { UserId = user.Id, RoleId = 3 }, CancellationToken.None);

            var updated = await _context.Users.FindAsync(user.Id);
            Assert.Equal(3, updated!.RoleId);
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenCallerIsNotAdmin()
        {
            var user = await SeedUserAsync(1, roleId: 2);
            _currentUserMock.Setup(x => x.RoleId).Returns(2);

            await Assert.ThrowsAsync<PawPalConflictException>(
                () => _sut.Handle(new UpdateUserRoleCommand { UserId = user.Id, RoleId = 3 }, CancellationToken.None));

            var unchanged = await _context.Users.FindAsync(user.Id);
            Assert.Equal(2, unchanged!.RoleId);
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenCallerIsNotAuthenticated()
        {
            var user = await SeedUserAsync(1, roleId: 2);
            _currentUserMock.Setup(x => x.RoleId).Returns((int?)null);

            await Assert.ThrowsAsync<PawPalConflictException>(
                () => _sut.Handle(new UpdateUserRoleCommand { UserId = user.Id, RoleId = 3 }, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFound_WhenUserDoesNotExist()
        {
            _currentUserMock.Setup(x => x.RoleId).Returns(3);

            await Assert.ThrowsAsync<PawPalNotFoundException>(
                () => _sut.Handle(new UpdateUserRoleCommand { UserId = 999, RoleId = 3 }, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowConflict_WhenRoleDoesNotExist()
        {
            var user = await SeedUserAsync(1, roleId: 2);
            _currentUserMock.Setup(x => x.RoleId).Returns(3);

            await Assert.ThrowsAsync<PawPalConflictException>(
                () => _sut.Handle(new UpdateUserRoleCommand { UserId = user.Id, RoleId = 999 }, CancellationToken.None));
        }
    }
}
