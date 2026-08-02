using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Modules.Master.Core.Features.Products.Commands.CreateProduct;
using Modules.Master.Core.Features.Products.Commands.DeleteProduct;
using Modules.Master.Core.Features.Products.Commands.UpdateProduct;
using Modules.Master.Core.Features.Products.Commands.UploadProductImage;
using Modules.Master.Core.Features.Products.Queries.GetProductById;
using Modules.Master.Core.Features.Products.Queries.GetProducts;

namespace Modules.Master.Controllers;

[ApiController]
[Authorize]
[Route("api/master/products")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductCommand command, CancellationToken cancellationToken)
        => Ok(await _mediator.Send(command, cancellationToken));

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProductCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
        {
            return BadRequest("Route id does not match request body id.");
        }

        return Ok(await _mediator.Send(command, cancellationToken));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        => Ok(await _mediator.Send(new DeleteProductCommand(id), cancellationToken));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        => Ok(await _mediator.Send(new GetProductByIdQuery(id), cancellationToken));

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetProductsQuery query, CancellationToken cancellationToken)
        => Ok(await _mediator.Send(query, cancellationToken));

    /// <summary>Uploads a product image to Cloudinary. Pass productId to also save the returned URL onto that product.</summary>
    [HttpPost("upload-image")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadImage(IFormFile file, [FromQuery] int? productId, CancellationToken cancellationToken)
    {
        if (file.Length == 0)
        {
            return BadRequest("No file was uploaded.");
        }

        await using var stream = file.OpenReadStream();
        var command = new UploadProductImageCommand(productId, stream, file.FileName);

        return Ok(await _mediator.Send(command, cancellationToken));
    }
}
