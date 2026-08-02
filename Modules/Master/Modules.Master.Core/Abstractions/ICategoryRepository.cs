using Modules.Master.Core.Entities;
using Shared.Core.Repositories;

namespace Modules.Master.Core.Abstractions;

public interface ICategoryRepository : IRepository<Category>
{
    Task<bool> NameExistsAsync(string categoryName, int? excludeId = null, CancellationToken cancellationToken = default);

    Task<bool> HasProductsAsync(int categoryId, CancellationToken cancellationToken = default);

    Task<int> GetProductCountAsync(int categoryId, CancellationToken cancellationToken = default);

    Task<List<Category>> GetAllActiveAsync(CancellationToken cancellationToken = default);
}
