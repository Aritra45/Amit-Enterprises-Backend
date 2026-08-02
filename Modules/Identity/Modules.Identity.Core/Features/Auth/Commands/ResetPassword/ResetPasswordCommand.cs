using MediatR;
using Shared.Core.Wrapper;

namespace Modules.Identity.Core.Features.Auth.Commands.ResetPassword;

public record ResetPasswordCommand(string Email, string Otp, string NewPassword) : IRequest<IResult>;
