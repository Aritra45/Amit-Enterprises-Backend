using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.Booking.Core.Features.Reports.Queries.DailySalesReport;
using Modules.Booking.Core.Features.Reports.Queries.ExpenseReport;
using Modules.Booking.Core.Features.Reports.Queries.LowStockReport;
using Modules.Booking.Core.Features.Reports.Queries.MonthlySalesReport;
using Modules.Booking.Core.Features.Reports.Queries.ProductSalesReport;
using Modules.Booking.Core.Features.Reports.Queries.ProfitReport;

namespace Modules.Booking.Controllers;

[ApiController]
[Authorize]
[Route("api/booking/reports")]
public class ReportsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReportsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("daily-sales")]
    public async Task<IActionResult> DailySales([FromQuery] DateTime fromDate, [FromQuery] DateTime toDate, CancellationToken cancellationToken)
        => Ok(await _mediator.Send(new GetDailySalesReportQuery(fromDate, toDate), cancellationToken));

    [HttpGet("monthly-sales")]
    public async Task<IActionResult> MonthlySales([FromQuery] int year, CancellationToken cancellationToken)
        => Ok(await _mediator.Send(new GetMonthlySalesReportQuery(year), cancellationToken));

    [HttpGet("product-sales")]
    public async Task<IActionResult> ProductSales([FromQuery] DateTime fromDate, [FromQuery] DateTime toDate, CancellationToken cancellationToken)
        => Ok(await _mediator.Send(new GetProductSalesReportQuery(fromDate, toDate), cancellationToken));

    [HttpGet("expenses")]
    public async Task<IActionResult> Expenses([FromQuery] DateTime fromDate, [FromQuery] DateTime toDate, CancellationToken cancellationToken)
        => Ok(await _mediator.Send(new GetExpenseReportQuery(fromDate, toDate), cancellationToken));

    [HttpGet("profit")]
    public async Task<IActionResult> Profit([FromQuery] DateTime fromDate, [FromQuery] DateTime toDate, CancellationToken cancellationToken)
        => Ok(await _mediator.Send(new GetProfitReportQuery(fromDate, toDate), cancellationToken));

    [HttpGet("low-stock")]
    public async Task<IActionResult> LowStock(CancellationToken cancellationToken)
        => Ok(await _mediator.Send(new GetLowStockReportQuery(), cancellationToken));
}
