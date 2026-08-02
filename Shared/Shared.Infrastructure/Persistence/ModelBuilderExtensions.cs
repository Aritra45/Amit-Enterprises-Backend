using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Shared.Core.Entities;

namespace Shared.Infrastructure.Persistence;

public static class ModelBuilderExtensions
{
    /// <summary>Applies a global "not deleted" query filter to every entity deriving from BaseEntity.</summary>
    public static void ApplySoftDeleteQueryFilter(this ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (!typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                continue;
            }

            var parameter = Expression.Parameter(entityType.ClrType, "e");
            var property = Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
            var comparison = Expression.Equal(property, Expression.Constant(false));
            var lambda = Expression.Lambda(comparison, parameter);

            modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
        }
    }
}
