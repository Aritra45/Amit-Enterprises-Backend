using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.Booking.Core.Features.Sales.Commands.CreateSale;
using Modules.Booking.Core.Features.Sales.Queries.GetSaleById;
using Modules.Booking.Core.Features.Sales.Queries.GetSales;

namespace Modules.Booking.Controllers;

[ApiController]
[Authorize]
[Route("api/booking/sales")]
public class SalesController : ControllerBase
{
    private readonly IMediator _mediator;

    public SalesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSaleCommand command, CancellationToken cancellationToken)
        => Ok(await _mediator.Send(command, cancellationToken));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        => Ok(await _mediator.Send(new GetSaleByIdQuery(id), cancellationToken));

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetSalesQuery query, CancellationToken cancellationToken)
        => Ok(await _mediator.Send(query, cancellationToken));
}
