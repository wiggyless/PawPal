using PawPal.Domain.Entities.Animal_Info;
using PawPal.Domain.Entities.Animal_Info.ManyToMany;
using PawPal.Domain.Entities.Catalog;
using PawPal.Domain.Entities.Identity;
using PawPal.Domain.Entities.Places;

namespace PawPal.Application.Abstractions;

// Application layer
public interface IAppDbContext
{
    DbSet<ProductEntity> Products { get; }
    DbSet<ProductCategoryEntity> ProductCategories { get; }
    DbSet<MarketUserEntity> Users { get; }
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

    Task<int> SaveChangesAsync(CancellationToken ct);
}