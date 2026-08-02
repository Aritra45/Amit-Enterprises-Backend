using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.Booking.Core.Features.Expenses.Commands.CreateExpense;
using Modules.Booking.Core.Features.Expenses.Commands.DeleteExpense;
using Modules.Booking.Core.Features.Expenses.Commands.UpdateExpense;
using Modules.Booking.Core.Features.Expenses.Queries.GetExpenseById;
using Modules.Booking.Core.Features.Expenses.Queries.GetExpenses;

namespace Modules.Booking.Controllers;

[ApiController]
[Authorize]
[Route("api/booking/expenses")]
public class ExpensesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ExpensesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateExpenseCommand command, CancellationToken cancellationToken)
        => Ok(await _mediator.Send(command, cancellationToken));

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateExpenseCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
        {
            return BadRequest("Route id does not match request body id.");
        }

        return Ok(await _mediator.Send(command, cancellationToken));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        => Ok(await _mediator.Send(new DeleteExpenseCommand(id), cancellationToken));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        => Ok(await _mediator.Send(new GetExpenseByIdQuery(id), cancellationToken));

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetExpensesQuery query, CancellationToken cancellationToken)
        => Ok(await _mediator.Send(query, cancellationToken));
}
