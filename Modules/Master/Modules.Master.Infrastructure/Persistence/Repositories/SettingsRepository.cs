using Microsoft.EntityFrameworkCore;
using Modules.Master.Core.Abstractions;
using Modules.Master.Core.Entities;
using Shared.Infrastructure.Persistence;

namespace Modules.Master.Infrastructure.Persistence.Repositories;

public class SettingsRepository : Repository<Settings, MasterDbContext>, ISettingsRepository
{
    public SettingsRepository(MasterDbContext context) : base(context)
    {
    }

    public async Task<Settings?> GetSettingsAsync(CancellationToken cancellationToken = default)
        => await DbSet.Where(s => !s.IsDeleted).FirstOrDefaultAsync(cancellationToken);
}
