using PawPal.Domain.Entities.Adoptions;

namespace PawPal.Infrastructure.Database.Configurations.Adoptions;

public sealed class AdoptionRequestConfiguration : IEntityTypeConfiguration<AdoptionRequestEntity>
{
    public void Configure(EntityTypeBuilder<AdoptionRequestEntity> b)
    {
        b.Property(x => x.Status)
            .HasConversion<string>();
    }
}
