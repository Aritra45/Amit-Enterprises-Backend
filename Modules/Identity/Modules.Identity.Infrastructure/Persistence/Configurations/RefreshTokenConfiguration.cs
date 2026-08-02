using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Identity.Core.Entities;

namespace Modules.Identity.Infrastructure.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");

        builder.HasKey(rt => rt.Id);

        builder.Property(rt => rt.Token)
            .IsRequired()
            .HasMaxLength(500);

        builder.HasIndex(rt => rt.Token).IsUnique();

        builder.Property(rt => rt.ReplacedByToken).HasMaxLength(500);

        builder.Property(rt => rt.CreatedBy).HasMaxLength(100);

        builder.Property(rt => rt.UpdatedBy).HasMaxLength(100);

        builder.Ignore(rt => rt.IsExpired);

        builder.Ignore(rt => rt.IsValid);
    }
}
