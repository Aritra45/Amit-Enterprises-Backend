using Microsoft.EntityFrameworkCore;
using Modules.Booking.Core.Abstractions;
using Modules.Booking.Core.Entities;
using Shared.Infrastructure.Persistence;

namespace Modules.Booking.Infrastructure.Persistence.Repositories;

public class StockAdjustmentRepository : Repository<StockAdjustment, BookingDbContext>, IStockAdjustmentRepository
{
    public StockAdjustmentRepository(BookingDbContext context) : base(context)
    {
    }

    public async Task<(List<StockAdjustment> Items, int TotalCount)> GetPagedAsync(
        int? productId,
        StockAdjustmentType? adjustmentType,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        IQueryable<StockAdjustment> query = DbSet.Where(a => !a.IsDeleted);

        if (productId.HasValue)
        {
            query = query.Where(a => a.ProductId == productId.Value);
        }

        if (adjustmentType.HasValue)
        {
            query = query.Where(a => a.AdjustmentType == adjustmentType.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        query = query.OrderByDescending(a => a.AdjustmentDate);

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
