using Microsoft.EntityFrameworkCore;
using Modules.Master.Core.Abstractions;
using Modules.Master.Core.Entities;
using Shared.Infrastructure.Persistence;

namespace Modules.Master.Infrastructure.Persistence.Repositories;

public class ProductRepository : Repository<Product, MasterDbContext>, IProductRepository
{
    public ProductRepository(MasterDbContext context) : base(context)
    {
    }

    public async Task<bool> ProductCodeExistsAsync(string productCode, int? excludeId = null, CancellationToken cancellationToken = default)
        => await DbSet.AnyAsync(
            p => p.ProductCode.ToLower() == productCode.ToLower() && !p.IsDeleted && (excludeId == null || p.Id != excludeId),
            cancellationToken);

    public async Task<bool> BarcodeExistsAsync(string barcode, int? excludeId = null, CancellationToken cancellationToken = default)
        => await DbSet.AnyAsync(
            p => p.Barcode != null && p.Barcode.ToLower() == barcode.ToLower() && !p.IsDeleted && (excludeId == null || p.Id != excludeId),
            cancellationToken);

    public async Task<Product?> GetByIdWithCategoryAsync(int id, CancellationToken cancellationToken = default)
        => await DbSet
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted, cancellationToken);

    public async Task<(List<Product> Items, int TotalCount)> GetPagedWithCategoryAsync(
        int? categoryId,
        bool? lowStockOnly,
        string? searchTerm,
        string? sortColumn,
        bool sortDescending,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Product> query = DbSet.Include(p => p.Category).Where(p => !p.IsDeleted);

        if (categoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == categoryId.Value);
        }

        if (lowStockOnly == true)
        {
            query = query.Where(p => p.CurrentStockQty <= p.MinStockQty);
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(p =>
                p.ProductName.Contains(searchTerm) ||
                p.ProductCode.Contains(searchTerm) ||
                (p.Barcode != null && p.Barcode.Contains(searchTerm)));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        query = sortColumn?.ToLower() switch
        {
            "productname" => sortDescending ? query.OrderByDescending(p => p.ProductName) : query.OrderBy(p => p.ProductName),
            "sellingprice" => sortDescending ? query.OrderByDescending(p => p.SellingPrice) : query.OrderBy(p => p.SellingPrice),
            "currentstockqty" => sortDescending ? query.OrderByDescending(p => p.CurrentStockQty) : query.OrderBy(p => p.CurrentStockQty),
            _ => sortDescending ? query.OrderByDescending(p => p.CreatedOn) : query.OrderBy(p => p.CreatedOn)
        };

        if (pageSize == -1)
        {
            var all = await query.ToListAsync(cancellationToken);
            return (all, totalCount);
        }

        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        pageSize = pageSize <= 0 ? 10 : pageSize;

        var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
        return (items, totalCount);
    }
}
