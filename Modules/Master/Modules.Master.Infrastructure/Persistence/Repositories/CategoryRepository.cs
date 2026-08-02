using Microsoft.EntityFrameworkCore;
using Modules.Master.Core.Abstractions;
using Modules.Master.Core.Entities;
using Shared.Infrastructure.Persistence;

namespace Modules.Master.Infrastructure.Persistence.Repositories;

public class CategoryRepository : Repository<Category, MasterDbContext>, ICategoryRepository
{
    public CategoryRepository(MasterDbContext context) : base(context)
    {
    }

    public async Task<bool> NameExistsAsync(string categoryName, int? excludeId = null, CancellationToken cancellationToken = default)
        => await DbSet.AnyAsync(
            c => c.CategoryName.ToLower() == categoryName.ToLower() && !c.IsDeleted && (excludeId == null || c.Id != excludeId),
            cancellationToken);

    public async Task<bool> HasProductsAsync(int categoryId, CancellationToken cancellationToken = default)
        => await Context.Products.AnyAsync(p => p.CategoryId == categoryId && !p.IsDeleted, cancellationToken);

    public async Task<int> GetProductCountAsync(int categoryId, CancellationToken cancellationToken = default)
        => await Context.Products.CountAsync(p => p.CategoryId == categoryId && !p.IsDeleted, cancellationToken);

    public async Task<List<Category>> GetAllActiveAsync(CancellationToken cancellationToken = default)
        => await DbSet
            .Where(c => !c.IsDeleted && c.IsActive)
            .OrderBy(c => c.CategoryName)
            .ToListAsync(cancellationToken);
}
