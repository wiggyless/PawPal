using PawPal.Domain.Entities.Animal_Info;
using PawPal.Domain.Entities.Catalog;
using PawPal.Domain.Entities.Identity;

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

    Task<int> SaveChangesAsync(CancellationToken ct);
}