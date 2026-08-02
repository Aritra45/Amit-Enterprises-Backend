using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Master.Core.Entities;

namespace Modules.Master.Infrastructure.Persistence.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.CategoryName)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(c => c.CategoryName).IsUnique();

        builder.Property(c => c.Description).HasMaxLength(500);

        builder.Property(c => c.CreatedBy).HasMaxLength(100);

        builder.Property(c => c.UpdatedBy).HasMaxLength(100);

        builder.HasMany(c => c.Products)
            .WithOne(p => p.Category)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
