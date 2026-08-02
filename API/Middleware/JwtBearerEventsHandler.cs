using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Shared.Core.Abstractions;

namespace API.Middleware;

/// <summary>
/// Custom JWT events so a logged-out access token is rejected immediately instead of staying valid
/// until it naturally expires (signed JWTs can't otherwise be revoked before their exp claim).
/// </summary>
public class JwtBearerEventsHandler : JwtBearerEvents
{
    private readonly ITokenBlacklistService _tokenBlacklistService;

    public JwtBearerEventsHandler(ITokenBlacklistService tokenBlacklistService)
    {
        _tokenBlacklistService = tokenBlacklistService;
    }

    public override Task TokenValidated(TokenValidatedContext context)
    {
        var jti = context.Principal?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;

        if (!string.IsNullOrEmpty(jti) && _tokenBlacklistService.IsBlacklisted(jti))
        {
            context.Fail("This token has been revoked.");
        }

        return Task.CompletedTask;
    }
}
