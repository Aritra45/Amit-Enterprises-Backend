using Shared.Core.Entities;

namespace Modules.Master.Core.Entities;

public class Category : BaseEntity
{
    public string CategoryName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public ICollection<Product> Products { get; set; } = new List<Product>();
}
