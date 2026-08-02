using Microsoft.EntityFrameworkCore;
using Modules.Master.Infrastructure.Persistence;
using Shared.Core.Abstractions;
using Shared.Core.Exceptions;
using Shared.DTOs.Common;

namespace Modules.Master.Infrastructure.Services;

/// <summary>Adapter exposing Master's product catalog to other modules (Booking) via the Shared.Core contract.</summary>
public class ProductCatalogService : IProductCatalogService
{
    private readonly MasterDbContext _context;

    public ProductCatalogService(MasterDbContext context)
    {
        _context = context;
    }

    public async Task<ProductCatalogDto?> GetByIdAsync(int productId, CancellationToken cancellationToken = default)
        => await _context.Products
            .Where(p => p.Id == productId && !p.IsDeleted)
            .Select(p => new ProductCatalogDto
            {
                Id = p.Id,
                ProductCode = p.ProductCode,
                Barcode = p.Barcode,
                ProductName = p.ProductName,
                PurchasePrice = p.PurchasePrice,
                SellingPrice = p.SellingPrice,
                GSTPercentage = p.GSTPercentage,
                CurrentStockQty = p.CurrentStockQty,
                MinStockQty = p.MinStockQty
            })
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<bool> ExistsAsync(int productId, CancellationToken cancellationToken = default)
        => await _context.Products.AnyAsync(p => p.Id == productId && !p.IsDeleted, cancellationToken);

    public async Task AdjustStockAsync(int productId, double quantityChange, CancellationToken cancellationToken = default)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId && !p.IsDeleted, cancellationToken)
            ?? throw new NotFoundException("Product", productId);

        product.CurrentStockQty += quantityChange;

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> GetTotalProductsCountAsync(CancellationToken cancellationToken = default)
        => await _context.Products.CountAsync(p => !p.IsDeleted, cancellationToken);

    public async Task<List<ProductCatalogDto>> GetLowStockProductsAsync(int take, CancellationToken cancellationToken = default)
        => await _context.Products
            .Where(p => !p.IsDeleted && p.CurrentStockQty <= p.MinStockQty)
            .OrderBy(p => p.CurrentStockQty)
            .Take(take)
            .Select(p => new ProductCatalogDto
            {
                Id = p.Id,
                ProductCode = p.ProductCode,
                Barcode = p.Barcode,
                ProductName = p.ProductName,
                PurchasePrice = p.PurchasePrice,
                SellingPrice = p.SellingPrice,
                GSTPercentage = p.GSTPercentage,
                CurrentStockQty = p.CurrentStockQty,
                MinStockQty = p.MinStockQty
            })
            .ToListAsync(cancellationToken);
}
