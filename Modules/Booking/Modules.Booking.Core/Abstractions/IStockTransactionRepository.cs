using Modules.Booking.Core.Entities;
using Shared.Core.Repositories;

namespace Modules.Booking.Core.Abstractions;

public interface IStockTransactionRepository : IRepository<StockTransaction>
{
    Task<(List<StockTransaction> Items, int TotalCount)> GetPagedAsync(
        int? productId,
        StockTransactionType? transactionType,
        DateTime? fromDate,
        DateTime? toDate,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);
}
