using PawPal.Domain.Entities.Posts;

namespace PawPal.Infrastructure.Database.Configurations.Posts;

public sealed class PostsConfiguration : IEntityTypeConfiguration<PostsEntity>
{
    public void Configure(EntityTypeBuilder<PostsEntity> b)
    {
        b.Property(x => x.Status)
            .HasConversion<string>();
    }
}
