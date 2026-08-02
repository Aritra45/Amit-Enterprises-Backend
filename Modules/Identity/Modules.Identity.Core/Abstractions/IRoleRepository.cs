using Modules.Identity.Core.Entities;
using Shared.Core.Repositories;

namespace Modules.Identity.Core.Abstractions;

public interface IRoleRepository : IRepository<Role>
{
    Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}
