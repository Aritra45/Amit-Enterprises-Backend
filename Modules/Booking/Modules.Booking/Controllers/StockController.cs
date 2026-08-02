using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.Booking.Core.Features.StockManagement.Commands.CreateStockAdjustment;
using Modules.Booking.Core.Features.StockManagement.Queries.GetStockAdjustments;
using Modules.Booking.Core.Features.StockManagement.Queries.GetStockTransactions;

namespace Modules.Booking.Controllers;

[ApiController]
[Authorize]
[Route("api/booking/stock")]
public class StockController : ControllerBase
{
    private readonly IMediator _mediator;

    public StockController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Records an Opening Stock, Damaged Stock, Expired Stock, or Manual adjustment; always creates a matching StockTransaction.</summary>
    [HttpPost("adjustments")]
    public async Task<IActionResult> CreateAdjustment([FromBody] CreateStockAdjustmentCommand command, CancellationToken cancellationToken)
        => Ok(await _mediator.Send(command, cancellationToken));

    [HttpGet("adjustments")]
    public async Task<IActionResult> GetAdjustments([FromQuery] GetStockAdjustmentsQuery query, CancellationToken cancellationToken)
        => Ok(await _mediator.Send(query, cancellationToken));

    [HttpGet("transactions")]
    public async Task<IActionResult> GetTransactions([FromQuery] GetStockTransactionsQuery query, CancellationToken cancellationToken)
        => Ok(await _mediator.Send(query, cancellationToken));
}
