using Modules.Master.Core.Entities;
using Shared.Core.Repositories;

namespace Modules.Master.Core.Abstractions;

public interface IProductRepository : IRepository<Product>
{
    Task<bool> ProductCodeExistsAsync(string productCode, int? excludeId = null, CancellationToken cancellationToken = default);

    Task<bool> BarcodeExistsAsync(string barcode, int? excludeId = null, CancellationToken cancellationToken = default);

    Task<Product?> GetByIdWithCategoryAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Paged product listing (with Category eagerly loaded) supporting search, sort and category filter.</summary>
    Task<(List<Product> Items, int TotalCount)> GetPagedWithCategoryAsync(
        int? categoryId,
        bool? lowStockOnly,
        string? searchTerm,
        string? sortColumn,
        bool sortDescending,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);
}
