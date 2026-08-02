using MediatR;
using Shared.Core.Wrapper;

namespace Modules.Identity.Core.Features.Auth.Commands.Logout;

/// <summary>AccessTokenJti/AccessTokenExpiresOnUtc come from the caller's current bearer token so it can be blacklisted immediately.</summary>
public record LogoutCommand(string RefreshToken, string? AccessTokenJti, DateTime? AccessTokenExpiresOnUtc) : IRequest<IResult>;
