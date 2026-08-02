using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.Identity.Core.Features.Auth.Commands.ChangePassword;
using Modules.Identity.Core.Features.Auth.Commands.Login;
using Modules.Identity.Core.Features.Auth.Commands.Logout;
using Modules.Identity.Core.Features.Auth.Commands.RefreshToken;
using Modules.Identity.Core.Features.Auth.Commands.Register;
using Modules.Identity.Core.Features.Auth.Commands.UpdateProfile;
using Modules.Identity.Core.Features.Auth.Queries.GetProfile;
using Shared.Core.Constants;

namespace Modules.Identity.Controllers;

[ApiController]
[Route("api/identity/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    // [Authorize(Roles = Roles.SuperAdmin)]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command, CancellationToken cancellationToken)
        => Ok(await _mediator.Send(command, cancellationToken));

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken cancellationToken)
        => Ok(await _mediator.Send(command, cancellationToken));

    [HttpPost("refresh-token")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command, CancellationToken cancellationToken)
        => Ok(await _mediator.Send(command, cancellationToken));

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout([FromBody] LogoutRequest request, CancellationToken cancellationToken)
    {
        var jti = User.FindFirstValue(JwtRegisteredClaimNames.Jti);

        DateTime? expiresOnUtc = null;
        var expClaim = User.FindFirstValue(JwtRegisteredClaimNames.Exp);
        if (long.TryParse(expClaim, out var expSeconds))
        {
            expiresOnUtc = DateTimeOffset.FromUnixTimeSeconds(expSeconds).UtcDateTime;
        }

        var command = new LogoutCommand(request.RefreshToken, jti, expiresOnUtc);
        return Ok(await _mediator.Send(command, cancellationToken));
    }

    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command, CancellationToken cancellationToken)
        => Ok(await _mediator.Send(command, cancellationToken));

    [HttpPut("profile")]
    [Authorize]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileCommand command, CancellationToken cancellationToken)
        => Ok(await _mediator.Send(command, cancellationToken));

    [HttpGet("profile")]
    [Authorize]
    public async Task<IActionResult> GetProfile(CancellationToken cancellationToken)
        => Ok(await _mediator.Send(new GetProfileQuery(), cancellationToken));
}
