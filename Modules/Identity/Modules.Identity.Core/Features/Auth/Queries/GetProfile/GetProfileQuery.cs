using MediatR;
using Shared.Core.Wrapper;

namespace Modules.Identity.Core.Features.Auth.Queries.GetProfile;

public record GetProfileQuery : IRequest<Result<ProfileResponse>>;
