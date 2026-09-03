using PawPal.Domain.Entities.Security;

namespace PawPal.Infrastructure.Database.Configurations.Security;

public sealed class SecurityAnswersConfiguration : IEntityTypeConfiguration<SecurityAnswers>
{
    public void Configure(EntityTypeBuilder<SecurityAnswers> b)
    {
        b.ToTable("SecurityAnswers");

        b.HasKey(x => x.Id);

        b.HasIndex(x => new { x.UserId, x.QuestionID })
            .IsUnique();

        b.Property(x => x.Answer)
            .IsRequired();

        b.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId);

        b.HasOne(x => x.Question)
            .WithMany()
            .HasForeignKey(x => x.QuestionID);
    }
}
