using Microsoft.EntityFrameworkCore;
using Modules.Booking.Core.Abstractions;
using Modules.Booking.Core.Entities;
using Shared.Infrastructure.Persistence;

namespace Modules.Booking.Infrastructure.Persistence.Repositories;

public class ExpenseRepository : Repository<Expense, BookingDbContext>, IExpenseRepository
{
    public ExpenseRepository(BookingDbContext context) : base(context)
    {
    }

    public async Task<(List<Expense> Items, int TotalCount)> GetPagedAsync(
        DateTime? fromDate,
        DateTime? toDate,
        string? category,
        string? searchTerm,
        string? sortColumn,
        bool sortDescending,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Expense> query = DbSet.Where(e => !e.IsDeleted);

        if (fromDate.HasValue)
        {
            query = query.Where(e => e.ExpenseDate >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            query = query.Where(e => e.ExpenseDate < toDate.Value);
        }

        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(e => e.Category == category);
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(e => e.Title.Contains(searchTerm) || (e.Notes != null && e.Notes.Contains(searchTerm)));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        query = sortColumn?.ToLower() switch
        {
            "amount" => sortDescending ? query.OrderByDescending(e => e.Amount) : query.OrderBy(e => e.Amount),
            "title" => sortDescending ? query.OrderByDescending(e => e.Title) : query.OrderBy(e => e.Title),
            _ => sortDescending ? query.OrderByDescending(e => e.ExpenseDate) : query.OrderBy(e => e.ExpenseDate)
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

    public async Task<double> GetTotalExpensesAsync(DateTime fromDateUtc, DateTime toDateUtc, CancellationToken cancellationToken = default)
        => await DbSet
            .Where(e => !e.IsDeleted && e.ExpenseDate >= fromDateUtc && e.ExpenseDate < toDateUtc)
            .SumAsync(e => (double?)e.Amount, cancellationToken) ?? 0;

    public async Task<List<Expense>> GetExpensesBetweenAsync(DateTime fromDateUtc, DateTime toDateUtc, CancellationToken cancellationToken = default)
        => await DbSet
            .Where(e => !e.IsDeleted && e.ExpenseDate >= fromDateUtc && e.ExpenseDate < toDateUtc)
            .ToListAsync(cancellationToken);
}
