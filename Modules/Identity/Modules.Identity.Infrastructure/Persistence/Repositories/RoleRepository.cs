using Microsoft.EntityFrameworkCore;
using Modules.Identity.Core.Abstractions;
using Modules.Identity.Core.Entities;
using Shared.Infrastructure.Persistence;

namespace Modules.Identity.Infrastructure.Persistence.Repositories;

public class RoleRepository : Repository<Role, IdentityDbContext>, IRoleRepository
{
    public RoleRepository(IdentityDbContext context) : base(context)
    {
    }

    public async Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        => await DbSet.FirstOrDefaultAsync(r => r.Name == name && !r.IsDeleted, cancellationToken);
}
