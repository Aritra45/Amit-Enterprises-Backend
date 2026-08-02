using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Booking.Core.Entities;

namespace Modules.Booking.Infrastructure.Persistence.Configurations;

public class StockAdjustmentConfiguration : IEntityTypeConfiguration<StockAdjustment>
{
    public void Configure(EntityTypeBuilder<StockAdjustment> builder)
    {
        builder.ToTable("StockAdjustments");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.ProductName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(a => a.AdjustmentType)
            .HasConversion<string>()
            .HasMaxLength(30);

        builder.Property(a => a.Reason).HasMaxLength(500);

        builder.Property(a => a.CreatedBy).HasMaxLength(100);

        builder.Property(a => a.UpdatedBy).HasMaxLength(100);

        builder.HasIndex(a => a.ProductId);
    }
}
