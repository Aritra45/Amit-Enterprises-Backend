using MediatR;
using Modules.Booking.Core.Abstractions;
using Shared.Core.Wrapper;

namespace Modules.Booking.Core.Features.Reports.Queries.MonthlySalesReport;

public class GetMonthlySalesReportQueryHandler : IRequestHandler<GetMonthlySalesReportQuery, Result<List<MonthlySalesReportItem>>>
{
    private readonly ISaleRepository _saleRepository;

    public GetMonthlySalesReportQueryHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public async Task<Result<List<MonthlySalesReportItem>>> Handle(GetMonthlySalesReportQuery request, CancellationToken cancellationToken)
    {
        var projections = await _saleRepository.GetMonthlySalesAsync(request.Year, cancellationToken);

        var items = projections.Select(p => new MonthlySalesReportItem
        {
            Year = p.Year,
            Month = p.Month,
            TotalSales = p.TotalSales,
            OrderCount = p.OrderCount
        }).ToList();

        return Result<List<MonthlySalesReportItem>>.Success(items);
    }
}
