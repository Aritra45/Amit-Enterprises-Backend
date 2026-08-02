using Microsoft.EntityFrameworkCore;
using Modules.Booking.Core.Abstractions;
using Modules.Booking.Core.Entities;
using Shared.Infrastructure.Persistence;

namespace Modules.Booking.Infrastructure.Persistence.Repositories;

public class SaleRepository : Repository<Sale, BookingDbContext>, ISaleRepository
{
    public SaleRepository(BookingDbContext context) : base(context)
    {
    }

    public async Task<string> GenerateNextInvoiceNumberAsync(CancellationToken cancellationToken = default)
    {
        var today = DateTime.UtcNow.Date;
        var tomorrow = today.AddDays(1);

        var todaysCount = await DbSet.CountAsync(s => s.SaleDate >= today && s.SaleDate < tomorrow, cancellationToken);

        return $"INV-{today:yyyyMMdd}-{todaysCount + 1:D4}";
    }

    public async Task<Sale?> GetByIdWithItemsAsync(int id, CancellationToken cancellationToken = default)
        => await DbSet
            .Include(s => s.SaleItems)
            .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted, cancellationToken);

    public async Task<(List<Sale> Items, int TotalCount)> GetPagedAsync(
        DateTime? fromDate,
        DateTime? toDate,
        string? invoiceSearch,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Sale> query = DbSet.Include(s => s.SaleItems).Where(s => !s.IsDeleted);

        if (fromDate.HasValue)
        {
            query = query.Where(s => s.SaleDate >= DateTime.SpecifyKind(fromDate.Value, DateTimeKind.Utc));
        }

        if (toDate.HasValue)
        {
            query = query.Where(s => s.SaleDate < DateTime.SpecifyKind(toDate.Value, DateTimeKind.Utc));
        }

        if (!string.IsNullOrWhiteSpace(invoiceSearch))
        {
            query = query.Where(s => s.InvoiceNumber.Contains(invoiceSearch));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        query = query.OrderByDescending(s => s.SaleDate);

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

    public async Task<double> GetTotalSalesAsync(DateTime fromDateUtc, DateTime toDateUtc, CancellationToken cancellationToken = default)
    {
        fromDateUtc = DateTime.SpecifyKind(fromDateUtc, DateTimeKind.Utc);
        toDateUtc = DateTime.SpecifyKind(toDateUtc, DateTimeKind.Utc);

        return await DbSet
            .Where(s => !s.IsDeleted && s.SaleDate >= fromDateUtc && s.SaleDate < toDateUtc)
            .SumAsync(s => (double?)s.GrandTotal, cancellationToken) ?? 0;
    }

    public async Task<int> GetOrdersCountAsync(DateTime fromDateUtc, DateTime toDateUtc, CancellationToken cancellationToken = default)
    {
        fromDateUtc = DateTime.SpecifyKind(fromDateUtc, DateTimeKind.Utc);
        toDateUtc = DateTime.SpecifyKind(toDateUtc, DateTimeKind.Utc);

        return await DbSet.CountAsync(s => !s.IsDeleted && s.SaleDate >= fromDateUtc && s.SaleDate < toDateUtc, cancellationToken);
    }

    public async Task<List<Sale>> GetRecentSalesAsync(int take, CancellationToken cancellationToken = default)
        => await DbSet
            .Where(s => !s.IsDeleted)
            .OrderByDescending(s => s.SaleDate)
            .Take(take)
            .ToListAsync(cancellationToken);

    public async Task<List<TopSellingProductProjection>> GetTopSellingProductsAsync(
        int take,
        DateTime? fromDateUtc = null,
        DateTime? toDateUtc = null,
        CancellationToken cancellationToken = default)
    {
        var query = Context.SaleItems
            .Where(i => !i.IsDeleted && !i.Sale.IsDeleted);

        if (fromDateUtc.HasValue)
        {
            query = query.Where(i => i.Sale.SaleDate >= DateTime.SpecifyKind(fromDateUtc.Value, DateTimeKind.Utc));
        }

        if (toDateUtc.HasValue)
        {
            query = query.Where(i => i.Sale.SaleDate < DateTime.SpecifyKind(toDateUtc.Value, DateTimeKind.Utc));
        }

        // Grouped aggregates are projected into a plain anonymous type first because EF Core's
        // SQL Server provider cannot reliably translate a GroupBy().Select() that constructs a
        // custom record via its positional constructor - it only translates simple member access,
        // Sum/Count and anonymous types. The final record is built client-side after ToListAsync.
        var grouped = await query
            .GroupBy(i => new { i.ProductId, i.ProductName })
            .Select(g => new
            {
                g.Key.ProductId,
                g.Key.ProductName,
                QuantitySold = g.Sum(i => i.Quantity),
                Revenue = g.Sum(i => i.TotalAmount)
            })
            .OrderByDescending(p => p.QuantitySold)
            .Take(take)
            .ToListAsync(cancellationToken);

        return grouped
            .Select(p => new TopSellingProductProjection(p.ProductId, p.ProductName, p.QuantitySold, p.Revenue))
            .ToList();
    }

    public async Task<List<ProductSalesProjection>> GetProductSalesAsync(DateTime fromDateUtc, DateTime toDateUtc, CancellationToken cancellationToken = default)
    {
        fromDateUtc = DateTime.SpecifyKind(fromDateUtc, DateTimeKind.Utc);
        toDateUtc = DateTime.SpecifyKind(toDateUtc, DateTimeKind.Utc);

        var grouped = await Context.SaleItems
            .Where(i => !i.IsDeleted && !i.Sale.IsDeleted && i.Sale.SaleDate >= fromDateUtc && i.Sale.SaleDate < toDateUtc)
            .GroupBy(i => new { i.ProductId, i.ProductName, i.ProductCode })
            .Select(g => new
            {
                g.Key.ProductId,
                g.Key.ProductName,
                g.Key.ProductCode,
                QuantitySold = g.Sum(i => i.Quantity),
                Revenue = g.Sum(i => i.TotalAmount)
            })
            .OrderByDescending(p => p.Revenue)
            .ToListAsync(cancellationToken);

        return grouped
            .Select(p => new ProductSalesProjection(p.ProductId, p.ProductName, p.ProductCode, p.QuantitySold, p.Revenue))
            .ToList();
    }

    public async Task<List<DailySalesProjection>> GetDailySalesAsync(DateTime fromDateUtc, DateTime toDateUtc, CancellationToken cancellationToken = default)
    {
        fromDateUtc = DateTime.SpecifyKind(fromDateUtc, DateTimeKind.Utc);
        toDateUtc = DateTime.SpecifyKind(toDateUtc, DateTimeKind.Utc);

        // SaleDate is stored in UTC, but the shop's "day" is an IST calendar day. Shifting by
        // the IST offset before taking .Date buckets each sale under the correct IST day
        // instead of the UTC day, which could be off by one near midnight IST.
        var grouped = await DbSet
            .Where(s => !s.IsDeleted && s.SaleDate >= fromDateUtc && s.SaleDate < toDateUtc)
            .GroupBy(s => s.SaleDate.AddHours(5).AddMinutes(30).Date)
            .Select(g => new { Date = g.Key, TotalSales = g.Sum(s => s.GrandTotal), OrderCount = g.Count() })
            .OrderBy(p => p.Date)
            .ToListAsync(cancellationToken);

        return grouped
            .Select(p => new DailySalesProjection(p.Date, p.TotalSales, p.OrderCount))
            .ToList();
    }

    public async Task<List<MonthlySalesProjection>> GetMonthlySalesAsync(int year, CancellationToken cancellationToken = default)
    {
        // Same IST-shift reasoning as GetDailySalesAsync, applied to the year/month grouping key.
        var grouped = await DbSet
            .Where(s => !s.IsDeleted && s.SaleDate.AddHours(5).AddMinutes(30).Year == year)
            .GroupBy(s => new { Year = s.SaleDate.AddHours(5).AddMinutes(30).Year, Month = s.SaleDate.AddHours(5).AddMinutes(30).Month })
            .Select(g => new { g.Key.Year, g.Key.Month, TotalSales = g.Sum(s => s.GrandTotal), OrderCount = g.Count() })
            .OrderBy(p => p.Month)
            .ToListAsync(cancellationToken);

        return grouped
            .Select(p => new MonthlySalesProjection(p.Year, p.Month, p.TotalSales, p.OrderCount))
            .ToList();
    }

    public async Task<double> GetTotalRevenueAsync(DateTime fromDateUtc, DateTime toDateUtc, CancellationToken cancellationToken = default)
        => await GetTotalSalesAsync(fromDateUtc, toDateUtc, cancellationToken);

    public async Task<double> GetTotalCostOfGoodsSoldAsync(DateTime fromDateUtc, DateTime toDateUtc, CancellationToken cancellationToken = default)
    {
        fromDateUtc = DateTime.SpecifyKind(fromDateUtc, DateTimeKind.Utc);
        toDateUtc = DateTime.SpecifyKind(toDateUtc, DateTimeKind.Utc);

        return await Context.SaleItems
            .Where(i => !i.IsDeleted && !i.Sale.IsDeleted && i.Sale.SaleDate >= fromDateUtc && i.Sale.SaleDate < toDateUtc)
            .SumAsync(i => (double?)(i.PurchasePrice * i.Quantity), cancellationToken) ?? 0;
    }
}
