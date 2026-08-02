using MediatR;
using Shared.Core.Wrapper;

namespace Modules.Identity.Core.Features.Auth.Commands.ForgotPassword;

public record ForgotPasswordCommand(string Email) : IRequest<IResult>;
