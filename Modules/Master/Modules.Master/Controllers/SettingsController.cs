using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Modules.Master.Core.Features.Settings.Commands.UpdateSettings;
using Modules.Master.Core.Features.Settings.Commands.UploadLogo;
using Modules.Master.Core.Features.Settings.Queries.GetSettings;

namespace Modules.Master.Controllers;

[ApiController]
[Authorize]
[Route("api/master/settings")]
public class SettingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public SettingsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
        => Ok(await _mediator.Send(new GetSettingsQuery(), cancellationToken));

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateSettingsCommand command, CancellationToken cancellationToken)
        => Ok(await _mediator.Send(command, cancellationToken));

    [HttpPost("upload-logo")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadLogo(IFormFile file, CancellationToken cancellationToken)
    {
        if (file.Length == 0)
        {
            return BadRequest("No file was uploaded.");
        }

        await using var stream = file.OpenReadStream();
        var command = new UploadLogoCommand(stream, file.FileName);

        return Ok(await _mediator.Send(command, cancellationToken));
    }
}
