using Modules.Booking.Core.Entities;
using Shared.Core.Repositories;

namespace Modules.Booking.Core.Abstractions;

public interface IStockAdjustmentRepository : IRepository<StockAdjustment>
{
    Task<(List<StockAdjustment> Items, int TotalCount)> GetPagedAsync(
        int? productId,
        StockAdjustmentType? adjustmentType,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);
}
