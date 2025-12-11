using PawPal.Application.Abstractions;
using PawPal.Domain.Entities.Adoptions;
using PawPal.Domain.Entities.Animal_Info;
using PawPal.Domain.Entities.Animal_Info.ManyToMany;
using PawPal.Domain.Entities.Catalog;
using PawPal.Domain.Entities.Identity;
using PawPal.Domain.Entities.Messaging;
using PawPal.Domain.Entities.News;
using PawPal.Domain.Entities.Places;
using PawPal.Domain.Entities.Posts;
namespace Market.Infrastructure.Database;

public partial class DatabaseContext : DbContext, IAppDbContext
{
    public DbSet<ProductCategoryEntity> ProductCategories => Set<ProductCategoryEntity>();
    public DbSet<ProductEntity> Products => Set<ProductEntity>();
    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<GenderEntity> Genders => Set<GenderEntity>();
    public DbSet<AnimalCategoriesEntity> AnimalCategories => Set<AnimalCategoriesEntity>();
    public DbSet<AnimalEntity> Animals => Set<AnimalEntity>();
    public DbSet<RefreshTokenEntity> RefreshTokens => Set<RefreshTokenEntity>();
    public DbSet<CitiesEntity> Cities => Set<CitiesEntity>();
    public DbSet<CantonEntity> Cantons => Set<CantonEntity>();
    public DbSet<AnimalHealthHistoryEntity> AnimalHealthHistories => Set<AnimalHealthHistoryEntity>();
    public DbSet<AllergiesEntity> Allergies => Set<AllergiesEntity>();
    public DbSet<DisabilitiesEntity> Disabilities => Set<DisabilitiesEntity>();
    public DbSet<AllergiesAnimalHealthHistory> AnimalsAllergies => Set<AllergiesAnimalHealthHistory>();
    public DbSet<DisabilitiesAnimalHealthHistory> AnimalsDisabilities => Set<DisabilitiesAnimalHealthHistory>();
    public DbSet<RolesEntity> Roles => Set<RolesEntity>();
    public DbSet<CharacteristicsEntity> Characteristics => Set<CharacteristicsEntity>();
    public DbSet<AnimalCharacteristics> AnimalCharacteristics => Set<AnimalCharacteristics>();
    public DbSet<AdoptionStoryEntity> AdoptionStories => Set<AdoptionStoryEntity>();
    public DbSet<AdoptionRequestEntity> AdoptionRequests => Set<AdoptionRequestEntity>();
    public DbSet<AdoptionRequirementEntity> AdoptionRequirements => Set<AdoptionRequirementEntity>();
    public DbSet<PostsEntity> Posts => Set<PostsEntity>();
    public DbSet<CommentsEntity> Comments => Set<CommentsEntity>();
    public DbSet<NewsEntity> News => Set<NewsEntity>();
    public DbSet<LikedUserPosts> LikedUserPosts => Set<LikedUserPosts>();
    public DbSet<UserToUserMessages> UserToUserMessages => Set<UserToUserMessages>();
    public DbSet<BreedEntity> Breeds => Set<BreedEntity>();

    private readonly TimeProvider _clock;
    public DatabaseContext(DbContextOptions<DatabaseContext> options, TimeProvider clock) : base(options)
    {
        _clock = clock;
    }
}