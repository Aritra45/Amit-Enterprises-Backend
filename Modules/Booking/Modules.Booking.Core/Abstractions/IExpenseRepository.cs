using Modules.Booking.Core.Entities;
using Shared.Core.Repositories;

namespace Modules.Booking.Core.Abstractions;

public interface IExpenseRepository : IRepository<Expense>
{
    Task<(List<Expense> Items, int TotalCount)> GetPagedAsync(
        DateTime? fromDate,
        DateTime? toDate,
        string? category,
        string? searchTerm,
        string? sortColumn,
        bool sortDescending,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<double> GetTotalExpensesAsync(DateTime fromDateUtc, DateTime toDateUtc, CancellationToken cancellationToken = default);

    Task<List<Expense>> GetExpensesBetweenAsync(DateTime fromDateUtc, DateTime toDateUtc, CancellationToken cancellationToken = default);
}
