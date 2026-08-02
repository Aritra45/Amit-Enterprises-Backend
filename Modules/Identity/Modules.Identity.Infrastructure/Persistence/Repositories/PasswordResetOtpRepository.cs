using Microsoft.EntityFrameworkCore;
using Modules.Identity.Core.Abstractions;
using Modules.Identity.Core.Entities;
using Shared.Infrastructure.Persistence;

namespace Modules.Identity.Infrastructure.Persistence.Repositories;

public class PasswordResetOtpRepository : Repository<PasswordResetOtp, IdentityDbContext>, IPasswordResetOtpRepository
{
    public PasswordResetOtpRepository(IdentityDbContext context) : base(context)
    {
    }

    public async Task<PasswordResetOtp?> GetLatestPendingForUserAsync(int userId, CancellationToken cancellationToken = default)
        => await DbSet
            .Where(o => o.UserId == userId && !o.IsUsed && !o.IsDeleted)
            .OrderByDescending(o => o.CreatedOn)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task InvalidatePendingForUserAsync(int userId, CancellationToken cancellationToken = default)
    {
        var pending = await DbSet
            .Where(o => o.UserId == userId && !o.IsUsed && !o.IsDeleted)
            .ToListAsync(cancellationToken);

        foreach (var otp in pending)
        {
            otp.IsUsed = true;
        }
    }
}
