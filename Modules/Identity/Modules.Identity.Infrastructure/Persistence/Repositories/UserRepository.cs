using Microsoft.EntityFrameworkCore;
using Modules.Identity.Core.Abstractions;
using Modules.Identity.Core.Entities;
using Shared.Infrastructure.Persistence;

namespace Modules.Identity.Infrastructure.Persistence.Repositories;

public class UserRepository : Repository<User, IdentityDbContext>, IUserRepository
{
    public UserRepository(IdentityDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => await DbSet
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower() && !u.IsDeleted, cancellationToken);

    public async Task<User?> GetByIdWithRoleAsync(int id, CancellationToken cancellationToken = default)
        => await DbSet
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted, cancellationToken);

    public async Task<bool> EmailExistsAsync(string email, int? excludeUserId = null, CancellationToken cancellationToken = default)
        => await DbSet.AnyAsync(
            u => u.Email.ToLower() == email.ToLower() && !u.IsDeleted && (excludeUserId == null || u.Id != excludeUserId),
            cancellationToken);
}
