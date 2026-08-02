using Modules.Identity.Core.Entities;
using Shared.Core.Repositories;

namespace Modules.Identity.Core.Abstractions;

public interface IRefreshTokenRepository : IRepository<RefreshToken>
{
    Task<RefreshToken?> GetByTokenWithUserAsync(string token, CancellationToken cancellationToken = default);
}
