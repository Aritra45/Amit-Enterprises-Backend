using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Booking.Core.Entities;

namespace Modules.Booking.Infrastructure.Persistence.Configurations;

public class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        builder.ToTable("Expenses");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(e => e.Category).HasMaxLength(100);

        builder.Property(e => e.Notes).HasMaxLength(1000);

        builder.Property(e => e.CreatedBy).HasMaxLength(100);

        builder.Property(e => e.UpdatedBy).HasMaxLength(100);
    }
}
