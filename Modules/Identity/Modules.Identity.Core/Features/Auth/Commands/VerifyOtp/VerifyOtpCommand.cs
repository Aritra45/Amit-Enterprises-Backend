using MediatR;
using Shared.Core.Wrapper;

namespace Modules.Identity.Core.Features.Auth.Commands.VerifyOtp;

public record VerifyOtpCommand(string Email, string Otp) : IRequest<IResult>;
