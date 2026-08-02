using MediatR;
using Modules.Booking.Core.Abstractions;
using Shared.Core.Wrapper;

namespace Modules.Booking.Core.Features.Reports.Queries.ExpenseReport;

public class GetExpenseReportQueryHandler : IRequestHandler<GetExpenseReportQuery, Result<ExpenseReportResponse>>
{
    private readonly IExpenseRepository _expenseRepository;

    public GetExpenseReportQueryHandler(IExpenseRepository expenseRepository)
    {
        _expenseRepository = expenseRepository;
    }

    public async Task<Result<ExpenseReportResponse>> Handle(GetExpenseReportQuery request, CancellationToken cancellationToken)
    {
        var expenses = await _expenseRepository.GetExpensesBetweenAsync(request.FromDate, request.ToDate, cancellationToken);

        var response = new ExpenseReportResponse
        {
            TotalExpenses = expenses.Sum(e => e.Amount),
            ByCategory = expenses
                .GroupBy(e => string.IsNullOrWhiteSpace(e.Category) ? "Uncategorized" : e.Category)
                .Select(g => new ExpenseCategoryBreakdown { Category = g.Key, TotalAmount = g.Sum(e => e.Amount) })
                .OrderByDescending(c => c.TotalAmount)
                .ToList()
        };

        return Result<ExpenseReportResponse>.Success(response);
    }
}
