using Moq;
using PawPal.Application.Abstractions;
using PawPal.Application.Common.Exceptions;
using PawPal.Application.Modules.Users.Commands.Create;
using PawPal.Domain.Entities.Identity;
using PawPal.Domain.Entities.Places;
using System.ComponentModel.DataAnnotations;

namespace PawPal.Tests.UnitTests.User
{
    public class CreateUsersCommandHandlerTests : IDisposable
    {
        private readonly DatabaseContext _context;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly CreateUsersCommandHandler _handler;

        public CreateUsersCommandHandlerTests()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) 
                .Options;

            _context = new DatabaseContext(options, TimeProvider.System);
            _emailServiceMock = new Mock<IEmailService>();

            _handler = new CreateUsersCommandHandler(_context, _emailServiceMock.Object);
        }

        private static CreateUserCommand ValidCommand(int cityId) => new CreateUserCommand
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Password = "SecureP@ssw0rd!",
            BirthDate = new DateTime(1995, 5, 20),
            RoleID = 1,
            City = cityId,
            Username = "johndoe",
            AboutMe = "Dog lover"
        };

        [Fact]
        public async Task Handle_ValidCommand_CreatesUserAndReturnsId()
        {
            // Arrange
            var city = new CitiesEntity { Id = 1, Name = "Sarajevo" };
            _context.Cities.Add(city);
            await _context.SaveChangesAsync();

            var command = ValidCommand(city.Id);

            // Act
            var userId = await _handler.Handle(command, CancellationToken.None);

            // Assert
            var createdUser = await _context.Users.FindAsync(userId);
            Assert.NotNull(createdUser);
            Assert.Equal("John", createdUser!.FirstName);
            Assert.Equal("john.doe@example.com", createdUser.Email);
            Assert.NotEqual("SecureP@ssw0rd!", createdUser.PasswordHash); // must be hashed
            Assert.True(createdUser.IsEnabled);
            Assert.False(createdUser.IsEmailConfirmed);
            Assert.False(string.IsNullOrEmpty(createdUser.EmailConfirmationToken));
            Assert.Equal(1, createdUser.RoleId);
        }

        [Theory]
        [InlineData(null, "Doe", "john@example.com", "Password1")]
        [InlineData("John", null, "john@example.com", "Password1")]
        [InlineData("John", "Doe", null, "Password1")]
        [InlineData("John", "Doe", "john@example.com", null)]
        [InlineData("", "Doe", "john@example.com", "Password1")]
        [InlineData("John", "  ", "john@example.com", "Password1")]
        public async Task Handle_MissingRequiredField_ThrowsValidationException(
            string? firstName, string? lastName, string? email, string? password)
        {
            // Arrange
            var command = new CreateUserCommand
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = password,
                BirthDate = new DateTime(1995, 5, 20),
                RoleID = 1,
                City = 1,
                Username = "johndoe",
                AboutMe = null
            };

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(
                () => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_MissingBirthDate_ThrowsValidationException()
        {
            var command = ValidCommand(1);
            command.BirthDate = null;

            await Assert.ThrowsAsync<ValidationException>(
                () => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_NonExistentCity_ThrowsPawPalConflictException()
        {
            // Arrange: no city added to context
            var command = ValidCommand(cityId: 999);

            // Act & Assert
            await Assert.ThrowsAsync<PawPalConflictException>(
                () => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_EmailAlreadyUsed_ThrowsPawPalConflictException()
        {
            // Arrange
            var city = new CitiesEntity { Id = 1, Name = "Sarajevo" };
            _context.Cities.Add(city);
            _context.Users.Add(new UserEntity
            {
                Email = "john.doe@example.com",
                Username = "existinguser"
            });
            await _context.SaveChangesAsync();

            var command = ValidCommand(city.Id);

            // Act & Assert
            await Assert.ThrowsAsync<PawPalConflictException>(
                () => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ValidCommand_DoesNotPersistPlainTextPassword()
        {
            var city = new CitiesEntity { Id = 1, Name = "Sarajevo" };
            _context.Cities.Add(city);
            await _context.SaveChangesAsync();

            var command = ValidCommand(city.Id);

            var userId = await _handler.Handle(command, CancellationToken.None);

            var createdUser = await _context.Users.FindAsync(userId);
            Assert.NotNull(createdUser!.PasswordHash);
            Assert.NotEqual(command.Password, createdUser.PasswordHash);
        }

        [Fact]
        public async Task Handle_ValidCommand_SetsEmailConfirmationTokenWithExpiry()
        {
            var city = new CitiesEntity { Id = 1, Name = "Sarajevo" };
            _context.Cities.Add(city);
            await _context.SaveChangesAsync();

            var command = ValidCommand(city.Id);
            var before = DateTime.UtcNow;

            var userId = await _handler.Handle(command, CancellationToken.None);

            var createdUser = await _context.Users.FindAsync(userId);
            Assert.NotNull(createdUser!.EmailConfirmationToken);
            Assert.NotNull(createdUser.EmailConfirmationTokenExpiresAt);
            Assert.True(createdUser.EmailConfirmationTokenExpiresAt > before);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}