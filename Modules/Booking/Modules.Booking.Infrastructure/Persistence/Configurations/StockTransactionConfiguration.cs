using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Booking.Core.Entities;

namespace Modules.Booking.Infrastructure.Persistence.Configurations;

public class StockTransactionConfiguration : IEntityTypeConfiguration<StockTransaction>
{
    public void Configure(EntityTypeBuilder<StockTransaction> builder)
    {
        builder.ToTable("StockTransactions");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.ProductName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.TransactionType)
            .HasConversion<string>()
            .HasMaxLength(30);

        builder.Property(t => t.ReferenceNumber).HasMaxLength(50);

        builder.Property(t => t.Remarks).HasMaxLength(500);

        builder.Property(t => t.CreatedBy).HasMaxLength(100);

        builder.Property(t => t.UpdatedBy).HasMaxLength(100);

        builder.HasIndex(t => t.ProductId);
    }
}
