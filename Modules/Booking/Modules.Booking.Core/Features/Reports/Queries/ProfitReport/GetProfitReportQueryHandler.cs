using MediatR;
using Modules.Booking.Core.Abstractions;
using Shared.Core.Wrapper;

namespace Modules.Booking.Core.Features.Reports.Queries.ProfitReport;

public class GetProfitReportQueryHandler : IRequestHandler<GetProfitReportQuery, Result<ProfitReportResponse>>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IExpenseRepository _expenseRepository;

    public GetProfitReportQueryHandler(ISaleRepository saleRepository, IExpenseRepository expenseRepository)
    {
        _saleRepository = saleRepository;
        _expenseRepository = expenseRepository;
    }

    public async Task<Result<ProfitReportResponse>> Handle(GetProfitReportQuery request, CancellationToken cancellationToken)
    {
        var totalRevenue = await _saleRepository.GetTotalRevenueAsync(request.FromDate, request.ToDate, cancellationToken);
        var totalCogs = await _saleRepository.GetTotalCostOfGoodsSoldAsync(request.FromDate, request.ToDate, cancellationToken);
        var totalExpenses = await _expenseRepository.GetTotalExpensesAsync(request.FromDate, request.ToDate, cancellationToken);

        var grossProfit = totalRevenue - totalCogs;

        var response = new ProfitReportResponse
        {
            TotalRevenue = totalRevenue,
            TotalCostOfGoodsSold = totalCogs,
            GrossProfit = grossProfit,
            TotalExpenses = totalExpenses,
            NetProfit = grossProfit - totalExpenses
        };

        return Result<ProfitReportResponse>.Success(response);
    }
}
