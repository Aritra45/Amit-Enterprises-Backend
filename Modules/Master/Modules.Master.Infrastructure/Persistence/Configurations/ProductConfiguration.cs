using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Master.Core.Entities;

namespace Modules.Master.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.ProductCode)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(p => p.ProductCode).IsUnique();

        builder.Property(p => p.Barcode).HasMaxLength(50);

        builder.HasIndex(p => p.Barcode).IsUnique().HasFilter("\"Barcode\" IS NOT NULL");

        builder.Property(p => p.ProductName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.ProductImage).HasMaxLength(1000);

        builder.Property(p => p.CreatedBy).HasMaxLength(100);

        builder.Property(p => p.UpdatedBy).HasMaxLength(100);
    }
}
