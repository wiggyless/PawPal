using PawPal.Domain.Entities.Adoptions;
using PawPal.Domain.Entities.Animal_Info;
using PawPal.Domain.Entities.Animal_Info.ManyToMany;
using PawPal.Domain.Entities.Catalog;
using PawPal.Domain.Entities.Identity;
using PawPal.Domain.Entities.Messaging;
using PawPal.Domain.Entities.News;
using PawPal.Domain.Entities.Places;
using PawPal.Domain.Entities.Posts;

namespace PawPal.Application.Abstractions;

// Application layer
public interface IAppDbContext
{
    DbSet<ProductEntity> Products { get; }
    DbSet<ProductCategoryEntity> ProductCategories { get; }
    DbSet<UserEntity> Users { get; }
    DbSet<RefreshTokenEntity> RefreshTokens { get; }
    DbSet<AnimalCategoriesEntity> AnimalCategories { get; }
    DbSet<AnimalEntity> Animals { get; }
    DbSet<GenderEntity> Genders { get; }
    DbSet<CitiesEntity> Cities { get; }
    DbSet<CantonEntity> Cantons { get; }
    DbSet<AnimalHealthHistoryEntity> AnimalHealthHistories { get; }
    DbSet<AllergiesEntity> Allergies { get; }
    DbSet<DisabilitiesEntity> Disabilities { get; }
    DbSet<AllergiesAnimalHealthHistory> AnimalsAllergies { get; }
    DbSet<DisabilitiesAnimalHealthHistory> AnimalsDisabilities { get; }
    DbSet<RolesEntity> Roles { get; }
    DbSet<CharacteristicsEntity> Characteristics { get; }
    DbSet<AnimalCharacteristics> AnimalCharacteristics { get; }
    DbSet<CommentsEntity > Comments { get; }
    DbSet<PostsEntity> Posts { get; }
    DbSet<AdoptionStoryEntity> AdoptionStories { get; }
    DbSet<AdoptionRequestEntity> AdoptionRequests { get; }
    DbSet<AdoptionRequirementEntity> AdoptionRequirements { get; }
    DbSet<NewsEntity> News { get; }
    DbSet<LikedUserPosts> LikedUserPosts { get; }
    DbSet<UserToUserMessages> UserToUserMessages { get; }
    Task<int> SaveChangesAsync(CancellationToken ct);
}