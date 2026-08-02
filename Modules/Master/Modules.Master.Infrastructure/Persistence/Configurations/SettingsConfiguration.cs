using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Master.Core.Entities;

namespace Modules.Master.Infrastructure.Persistence.Configurations;

public class SettingsConfiguration : IEntityTypeConfiguration<Settings>
{
    public void Configure(EntityTypeBuilder<Settings> builder)
    {
        builder.ToTable("Settings");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.ShopName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(s => s.OwnerName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(s => s.MobileNumber).HasMaxLength(20);

        builder.Property(s => s.Address).HasMaxLength(500);

        builder.Property(s => s.GSTNumber).HasMaxLength(15);

        builder.Property(s => s.LogoUrl).HasMaxLength(1000);

        builder.Property(s => s.CreatedBy).HasMaxLength(100);

        builder.Property(s => s.UpdatedBy).HasMaxLength(100);
    }
}
