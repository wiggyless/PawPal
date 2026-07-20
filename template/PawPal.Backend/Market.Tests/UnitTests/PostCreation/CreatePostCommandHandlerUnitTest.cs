using Moq;
using PawPal.Application.Abstractions;
using PawPal.Application.Modules.Posts.Commands.Create;
using PawPal.Domain.Entities.Animal_Info;
using PawPal.Domain.Entities.Identity;
using PawPal.Domain.Entities.Posts;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class CreatePostCommandHandlerUnitTest
{
    private readonly DatabaseContext _context;
    private readonly Mock<IAppCurrentUser> _currentUserMock;
    private readonly CreatePostCommandHandler _sut;

    public CreatePostCommandHandlerUnitTest()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new DatabaseContext(options, TimeProvider.System);
        _currentUserMock = new Mock<IAppCurrentUser>();
        _sut = new CreatePostCommandHandler(_context, _currentUserMock.Object);
    }

    private async Task<UserEntity> SeedUserAsync(int cityId = 10)
    {
        var user = new UserEntity { Id = 1, Username = "Alice", FcmToken = "tokenA", CityId = cityId };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    private async Task<AnimalEntity> SeedAnimalAsync(int id = 1)
    {
        var animal = new AnimalEntity { Id = id  };
        _context.Animals.Add(animal);
        await _context.SaveChangesAsync();
        return animal;
    }

    private void SetupAuthorizedUser()
    {
        _currentUserMock.SetupGet(x => x.RoleId).Returns(2);
        _currentUserMock.SetupGet(x => x.IsAuthenticated).Returns(true);
    }

    [Fact]
    public async Task Handle_ShouldCreatePost_WhenRequestIsValid()
    {
        var user = await SeedUserAsync();
        var animal = await SeedAnimalAsync();
        SetupAuthorizedUser();

        var command = new CreatePostCommand
        {
            UserId = user.Id,
            AnimalID = animal.Id
        };

  
        var result = await _sut.Handle(command, CancellationToken.None);

        Assert.True(result > 0);
        var saved = await _context.Posts.FirstOrDefaultAsync(p => p.Id == result);
        Assert.NotNull(saved);
        Assert.Equal(user.Id, saved!.UserId);
        Assert.Equal(animal.Id, saved.AnimalID);
        Assert.Equal(user.CityId, saved.CityId);
        Assert.Equal("active", saved.Status);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenUserIsNotAuthenticated()
    {
        var user = await SeedUserAsync();
        var animal = await SeedAnimalAsync();

        _currentUserMock.SetupGet(x => x.RoleId).Returns(2);
        _currentUserMock.SetupGet(x => x.IsAuthenticated).Returns(false);

        var command = new CreatePostCommand { UserId = user.Id, AnimalID = animal.Id };

        await Assert.ThrowsAsync<Exception>(() => _sut.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenRoleIsNotAllowed()
    {
        var user = await SeedUserAsync();
        var animal = await SeedAnimalAsync();

        _currentUserMock.SetupGet(x => x.RoleId).Returns(1);
        _currentUserMock.SetupGet(x => x.IsAuthenticated).Returns(true);

        var command = new CreatePostCommand { UserId = user.Id, AnimalID = animal.Id };

        await Assert.ThrowsAsync<Exception>(() => _sut.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrowNullReference_WhenAnimalDoesNotExist()
    {
        var user = await SeedUserAsync();
        SetupAuthorizedUser();

        var command = new CreatePostCommand { UserId = user.Id, AnimalID = 999 };

        await Assert.ThrowsAsync<NullReferenceException>(
            () => _sut.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldSetHealthHistoryNull_WhenNoHealthHistoryExists()
    {
        var user = await SeedUserAsync();
        var animal = await SeedAnimalAsync();
        SetupAuthorizedUser();

        var command = new CreatePostCommand { UserId = user.Id, AnimalID = animal.Id };

        var result = await _sut.Handle(command, CancellationToken.None);

        var saved = await _context.Posts.FirstOrDefaultAsync(p => p.Id == result);
        Assert.Null(saved!.AnimalHealthHistory);
    }

    [Fact]
    public async Task Handle_ShouldAttachHealthHistory_WhenItExists()
    {
        var user = await SeedUserAsync();
        var animal = await SeedAnimalAsync();

        var health = new AnimalHealthHistoryEntity { AnimalId = animal.Id, Vaccinated = false, ParasiteFree = true,SpayedOrNeutered = false, DietaryRestrictions = "none" };
        _context.AnimalHealthHistories.Add(health);
        await _context.SaveChangesAsync();

        SetupAuthorizedUser();

        var command = new CreatePostCommand { UserId = user.Id, AnimalID = animal.Id };

        var result = await _sut.Handle(command, CancellationToken.None);

        var saved = await _context.Posts
            .Include(p => p.AnimalHealthHistory)
            .FirstOrDefaultAsync(p => p.Id == result);

        Assert.NotNull(saved!.AnimalHealthHistory);
        Assert.Equal(animal.Id, saved.AnimalHealthHistory!.AnimalId);
    }
}