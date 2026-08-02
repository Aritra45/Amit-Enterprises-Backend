using Shared.DTOs.Common;

namespace Shared.Core.Abstractions;

/// <summary>
/// Cross-module contract implemented by the Master module and consumed by the Booking module,
/// so Booking never takes a direct project reference on Master.
/// </summary>
public interface IProductCatalogService
{
    Task<ProductCatalogDto?> GetByIdAsync(int productId, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(int productId, CancellationToken cancellationToken = default);

    /// <summary>Applies a signed quantity change to CurrentStockQty (positive to add, negative to deduct).</summary>
    Task AdjustStockAsync(int productId, double quantityChange, CancellationToken cancellationToken = default);

    Task<int> GetTotalProductsCountAsync(CancellationToken cancellationToken = default);

    Task<List<ProductCatalogDto>> GetLowStockProductsAsync(int take, CancellationToken cancellationToken = default);
}
