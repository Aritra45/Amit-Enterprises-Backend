using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Booking.Core.Entities;

namespace Modules.Booking.Infrastructure.Persistence.Configurations;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable("Sales");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.InvoiceNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(s => s.InvoiceNumber).IsUnique();

        builder.Property(s => s.CustomerName).HasMaxLength(150);

        builder.Property(s => s.CustomerMobile).HasMaxLength(20);

        builder.Property(s => s.PaymentMode)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(s => s.CreatedBy).HasMaxLength(100);

        builder.Property(s => s.UpdatedBy).HasMaxLength(100);

        builder.HasMany(s => s.SaleItems)
            .WithOne(i => i.Sale)
            .HasForeignKey(i => i.SaleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
