using Microsoft.EntityFrameworkCore;
using Moq;
using PawPal.Application.Abstractions;
using PawPal.Application.Common.Exceptions;
using PawPal.Application.Modules.Users.Queries.GetById;
using PawPal.Application.Modules.Users.Queries.GetPublicProfile;
using PawPal.Domain.Entities.Identity;
using PawPal.Domain.Entities.Places;
using Xunit;

namespace PawPal.Tests.UnitTests.User
{
    public class UserProfileVisibilityUnitTest
    {
        private readonly DatabaseContext _context;
        private readonly Mock<IAppCurrentUser> _currentUserMock;

        public UserProfileVisibilityUnitTest()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new DatabaseContext(options, TimeProvider.System);
            _currentUserMock = new Mock<IAppCurrentUser>();

            var canton = new CantonEntity { Id = 1, FullName = "Sarajevo Canton", Abbreviation = "SA" };
            var city = new CitiesEntity { Id = 1, Name = "Sarajevo", CantonId = 1, Canton = canton };
            _context.Cantons.Add(canton);
            _context.Cities.Add(city);
            _context.SaveChanges();
        }

        private async Task<UserEntity> SeedUserAsync(int id, bool disabled = false)
        {
            var user = new UserEntity
            {
                Id = id,
                Username = $"user{id}",
                Email = $"user{id}@example.com",
                FirstName = "Jane",
                LastName = "Doe",
                CityId = 1,
                BirthDate = new DateTime(1995, 5, 20),
                isUserDisabled = disabled,
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        // ---- Full profile (GetUserByIdQuery): owner or admin only ----

        [Fact]
        public async Task GetById_ShouldReturnFullProfile_ForOwner()
        {
            var user = await SeedUserAsync(1);
            _currentUserMock.Setup(x => x.UserId).Returns(1);
            var sut = new GetUserByIdQueryHandler(_context, _currentUserMock.Object);

            var result = await sut.Handle(new GetUserByIdQuery { Id = user.Id }, CancellationToken.None);

            Assert.Equal("user1@example.com", result.Email);
        }

        [Fact]
        public async Task GetById_ShouldReturnFullProfile_ForAdmin()
        {
            var user = await SeedUserAsync(1);
            _currentUserMock.Setup(x => x.UserId).Returns(999);
            _currentUserMock.Setup(x => x.RoleId).Returns(3);
            var sut = new GetUserByIdQueryHandler(_context, _currentUserMock.Object);

            var result = await sut.Handle(new GetUserByIdQuery { Id = user.Id }, CancellationToken.None);

            Assert.Equal("user1@example.com", result.Email);
        }

        [Fact]
        public async Task GetById_ShouldThrow_ForAnotherRegularUser()
        {
            var user = await SeedUserAsync(1);
            _currentUserMock.Setup(x => x.UserId).Returns(2);
            _currentUserMock.Setup(x => x.RoleId).Returns(1);
            var sut = new GetUserByIdQueryHandler(_context, _currentUserMock.Object);

            await Assert.ThrowsAsync<PawPalConflictException>(
                () => sut.Handle(new GetUserByIdQuery { Id = user.Id }, CancellationToken.None));
        }

        [Fact]
        public async Task GetById_ShouldThrow_ForAnonymousCaller()
        {
            var user = await SeedUserAsync(1);
            _currentUserMock.Setup(x => x.UserId).Returns((int?)null);
            _currentUserMock.Setup(x => x.RoleId).Returns((int?)null);
            var sut = new GetUserByIdQueryHandler(_context, _currentUserMock.Object);

            await Assert.ThrowsAsync<PawPalConflictException>(
                () => sut.Handle(new GetUserByIdQuery { Id = user.Id }, CancellationToken.None));
        }

        // ---- Public profile (GetPublicUserProfileQuery): anyone, redacted fields only ----

        [Fact]
        public async Task GetPublicProfile_ShouldReturnRedactedProfile_ForAnyCaller()
        {
            var user = await SeedUserAsync(1);
            _currentUserMock.Setup(x => x.UserId).Returns((int?)null);
            var sut = new GetPublicUserProfileQueryHandler(_context, _currentUserMock.Object);

            var result = await sut.Handle(new GetPublicUserProfileQuery { Id = user.Id }, CancellationToken.None);

            Assert.Equal("Jane", result.FirstName);
            Assert.Equal("Sarajevo", result.City);
        }

        [Fact]
        public void GetPublicProfile_ShouldNotExposePrivateFields()
        {
            var dtoProperties = typeof(GetPublicUserProfileQueryDto).GetProperties().Select(p => p.Name);

            Assert.DoesNotContain("Email", dtoProperties);
            Assert.DoesNotContain("DateTime", dtoProperties);
            Assert.DoesNotContain("CantonAbbrevation", dtoProperties);
            Assert.DoesNotContain("Disabled", dtoProperties);
        }

        [Fact]
        public async Task GetPublicProfile_ShouldThrowNotFound_WhenUserIsDisabled_ForRegularCaller()
        {
            var user = await SeedUserAsync(1, disabled: true);
            _currentUserMock.Setup(x => x.UserId).Returns((int?)null);
            var sut = new GetPublicUserProfileQueryHandler(_context, _currentUserMock.Object);

            await Assert.ThrowsAsync<PawPalNotFoundException>(
                () => sut.Handle(new GetPublicUserProfileQuery { Id = user.Id }, CancellationToken.None));
        }

        [Fact]
        public async Task GetPublicProfile_ShouldReturnProfile_WhenUserIsDisabled_ForAdmin()
        {
            var user = await SeedUserAsync(1, disabled: true);
            _currentUserMock.Setup(x => x.RoleId).Returns(3);
            var sut = new GetPublicUserProfileQueryHandler(_context, _currentUserMock.Object);

            var result = await sut.Handle(new GetPublicUserProfileQuery { Id = user.Id }, CancellationToken.None);

            Assert.Equal(user.Id, result.Id);
        }
    }
}
