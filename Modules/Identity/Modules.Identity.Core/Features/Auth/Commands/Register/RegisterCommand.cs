using MediatR;
using Modules.Identity.Core.Features.Auth.Commands.Login;
using Shared.Core.Wrapper;

namespace Modules.Identity.Core.Features.Auth.Commands.Register;

public record RegisterCommand(
    string FullName,
    string Email,
    string Password,
    string? MobileNumber,
    int RoleId) : IRequest<Result<LoginResponse>>;
