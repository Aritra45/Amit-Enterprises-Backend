using Microsoft.EntityFrameworkCore;
using Modules.Identity.Core.Abstractions;
using Modules.Identity.Core.Entities;
using Shared.Infrastructure.Persistence;

namespace Modules.Identity.Infrastructure.Persistence.Repositories;

public class RefreshTokenRepository : Repository<RefreshToken, IdentityDbContext>, IRefreshTokenRepository
{
    public RefreshTokenRepository(IdentityDbContext context) : base(context)
    {
    }

    public async Task<RefreshToken?> GetByTokenWithUserAsync(string token, CancellationToken cancellationToken = default)
        => await DbSet
            .Include(rt => rt.User)
                .ThenInclude(u => u.Role)
            .FirstOrDefaultAsync(rt => rt.Token == token && !rt.IsDeleted, cancellationToken);
}
