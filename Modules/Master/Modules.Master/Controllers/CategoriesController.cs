using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.Master.Core.Features.Categories.Commands.CreateCategory;
using Modules.Master.Core.Features.Categories.Commands.DeleteCategory;
using Modules.Master.Core.Features.Categories.Commands.UpdateCategory;
using Modules.Master.Core.Features.Categories.Queries.GetCategories;
using Modules.Master.Core.Features.Categories.Queries.GetCategoryById;

namespace Modules.Master.Controllers;

[ApiController]
[Authorize]
[Route("api/master/categories")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command, CancellationToken cancellationToken)
        => Ok(await _mediator.Send(command, cancellationToken));

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
        {
            return BadRequest("Route id does not match request body id.");
        }

        return Ok(await _mediator.Send(command, cancellationToken));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        => Ok(await _mediator.Send(new DeleteCategoryCommand(id), cancellationToken));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        => Ok(await _mediator.Send(new GetCategoryByIdQuery(id), cancellationToken));

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetCategoriesQuery query, CancellationToken cancellationToken)
        => Ok(await _mediator.Send(query, cancellationToken));
}
