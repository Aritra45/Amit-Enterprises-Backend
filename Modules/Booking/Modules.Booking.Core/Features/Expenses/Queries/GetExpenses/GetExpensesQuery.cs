using MediatR;
using Shared.Core.Wrapper;
using Shared.DTOs.Pagination;

namespace Modules.Booking.Core.Features.Expenses.Queries.GetExpenses;

public class GetExpensesQuery : PaginationRequest, IRequest<PaginatedResult<ExpenseResponse>>
{
    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public string? Category { get; set; }
}
