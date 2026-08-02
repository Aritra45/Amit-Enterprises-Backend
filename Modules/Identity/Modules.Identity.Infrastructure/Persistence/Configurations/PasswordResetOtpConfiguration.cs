using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Identity.Core.Entities;

namespace Modules.Identity.Infrastructure.Persistence.Configurations;

public class PasswordResetOtpConfiguration : IEntityTypeConfiguration<PasswordResetOtp>
{
    public void Configure(EntityTypeBuilder<PasswordResetOtp> builder)
    {
        builder.ToTable("PasswordResetOtps");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.OtpHash)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(o => o.CreatedBy).HasMaxLength(100);

        builder.Property(o => o.UpdatedBy).HasMaxLength(100);

        builder.HasIndex(o => o.UserId);

        builder.HasOne(o => o.User)
            .WithMany()
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(o => o.IsExpired);
    }
}
