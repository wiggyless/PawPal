using PawPal.Application.Abstractions;
using PawPal.Domain.Entities.Animal_Info;
using PawPal.Domain.Entities.Catalog;
using PawPal.Domain.Entities.Identity;

namespace Market.Infrastructure.Database;

public partial class DatabaseContext : DbContext, IAppDbContext
{
    public DbSet<ProductCategoryEntity> ProductCategories => Set<ProductCategoryEntity>();
    public DbSet<ProductEntity> Products => Set<ProductEntity>();
    public DbSet<MarketUserEntity> Users => Set<MarketUserEntity>();
    public DbSet<GenderEntity> Genders => Set<GenderEntity>();
    public DbSet<AnimalCategoriesEntity> AnimalCategories => Set<AnimalCategoriesEntity>();
    public DbSet<AnimalEntity> Animals => Set<AnimalEntity>();
    public DbSet<RefreshTokenEntity> RefreshTokens => Set<RefreshTokenEntity>();

    private readonly TimeProvider _clock;
    public DatabaseContext(DbContextOptions<DatabaseContext> options, TimeProvider clock) : base(options)
    {
        _clock = clock;
    }
}