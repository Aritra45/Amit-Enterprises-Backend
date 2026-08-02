using Modules.Identity.Core.Entities;
using Shared.Core.Repositories;

namespace Modules.Identity.Core.Abstractions;

public interface IPasswordResetOtpRepository : IRepository<PasswordResetOtp>
{
    Task<PasswordResetOtp?> GetLatestPendingForUserAsync(int userId, CancellationToken cancellationToken = default);

    Task InvalidatePendingForUserAsync(int userId, CancellationToken cancellationToken = default);
}
