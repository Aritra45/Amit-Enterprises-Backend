using Modules.Master.Core.Entities;
using Shared.Core.Repositories;

namespace Modules.Master.Core.Abstractions;

public interface ISettingsRepository : IRepository<Settings>
{
    /// <summary>The shop has exactly one settings row.</summary>
    Task<Settings?> GetSettingsAsync(CancellationToken cancellationToken = default);
}
