using MediatR;
using Modules.Booking.Core.Abstractions;
using Shared.Core.Wrapper;

namespace Modules.Booking.Core.Features.Reports.Queries.DailySalesReport;

public class GetDailySalesReportQueryHandler : IRequestHandler<GetDailySalesReportQuery, Result<List<DailySalesReportItem>>>
{
    private readonly ISaleRepository _saleRepository;

    public GetDailySalesReportQueryHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public async Task<Result<List<DailySalesReportItem>>> Handle(GetDailySalesReportQuery request, CancellationToken cancellationToken)
    {
        var projections = await _saleRepository.GetDailySalesAsync(request.FromDate, request.ToDate, cancellationToken);

        var items = projections.Select(p => new DailySalesReportItem
        {
            Date = p.Date,
            TotalSales = p.TotalSales,
            OrderCount = p.OrderCount
        }).ToList();

        return Result<List<DailySalesReportItem>>.Success(items);
    }
}
