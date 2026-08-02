using Modules.Booking.Core.Entities;
using Shared.Core.Repositories;

namespace Modules.Booking.Core.Abstractions;

public interface ISaleRepository : IRepository<Sale>
{
    Task<string> GenerateNextInvoiceNumberAsync(CancellationToken cancellationToken = default);

    Task<Sale?> GetByIdWithItemsAsync(int id, CancellationToken cancellationToken = default);

    Task<(List<Sale> Items, int TotalCount)> GetPagedAsync(
        DateTime? fromDate,
        DateTime? toDate,
        string? invoiceSearch,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<double> GetTotalSalesAsync(DateTime fromDateUtc, DateTime toDateUtc, CancellationToken cancellationToken = default);

    Task<int> GetOrdersCountAsync(DateTime fromDateUtc, DateTime toDateUtc, CancellationToken cancellationToken = default);

    Task<List<Sale>> GetRecentSalesAsync(int take, CancellationToken cancellationToken = default);

    Task<List<TopSellingProductProjection>> GetTopSellingProductsAsync(int take, DateTime? fromDateUtc = null, DateTime? toDateUtc = null, CancellationToken cancellationToken = default);

    Task<List<ProductSalesProjection>> GetProductSalesAsync(DateTime fromDateUtc, DateTime toDateUtc, CancellationToken cancellationToken = default);

    Task<List<DailySalesProjection>> GetDailySalesAsync(DateTime fromDateUtc, DateTime toDateUtc, CancellationToken cancellationToken = default);

    Task<List<MonthlySalesProjection>> GetMonthlySalesAsync(int year, CancellationToken cancellationToken = default);

    Task<double> GetTotalRevenueAsync(DateTime fromDateUtc, DateTime toDateUtc, CancellationToken cancellationToken = default);

    Task<double> GetTotalCostOfGoodsSoldAsync(DateTime fromDateUtc, DateTime toDateUtc, CancellationToken cancellationToken = default);
}
