using Microsoft.EntityFrameworkCore;
using Modules.Booking.Core.Abstractions;
using Modules.Booking.Core.Entities;
using Shared.Infrastructure.Persistence;

namespace Modules.Booking.Infrastructure.Persistence.Repositories;

public class StockTransactionRepository : Repository<StockTransaction, BookingDbContext>, IStockTransactionRepository
{
    public StockTransactionRepository(BookingDbContext context) : base(context)
    {
    }

    public async Task<(List<StockTransaction> Items, int TotalCount)> GetPagedAsync(
        int? productId,
        StockTransactionType? transactionType,
        DateTime? fromDate,
        DateTime? toDate,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        IQueryable<StockTransaction> query = DbSet.Where(t => !t.IsDeleted);

        if (productId.HasValue)
        {
            query = query.Where(t => t.ProductId == productId.Value);
        }

        if (transactionType.HasValue)
        {
            query = query.Where(t => t.TransactionType == transactionType.Value);
        }

        if (fromDate.HasValue)
        {
            query = query.Where(t => t.CreatedOn >= DateTime.SpecifyKind(fromDate.Value, DateTimeKind.Utc));
        }

        if (toDate.HasValue)
        {
            query = query.Where(t => t.CreatedOn < DateTime.SpecifyKind(toDate.Value, DateTimeKind.Utc));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        query = query.OrderByDescending(t => t.CreatedOn);

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
