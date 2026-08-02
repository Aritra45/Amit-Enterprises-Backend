using MediatR;
using Modules.Identity.Core.Features.Auth.Commands.Login;
using Shared.Core.Wrapper;

namespace Modules.Identity.Core.Features.Auth.Commands.RefreshToken;

public record RefreshTokenCommand(string RefreshToken) : IRequest<Result<LoginResponse>>;
