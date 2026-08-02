using MediatR;
using Shared.Core.Wrapper;

namespace Modules.Identity.Core.Features.Auth.Commands.UpdateProfile;

public record UpdateProfileCommand(string FullName, string? MobileNumber) : IRequest<IResult>;
