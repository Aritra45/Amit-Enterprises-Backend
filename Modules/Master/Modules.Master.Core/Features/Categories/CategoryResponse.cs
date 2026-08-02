namespace Modules.Master.Core.Features.Categories;

public class CategoryResponse
{
    public int Id { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public int ProductCount { get; set; }

    public DateTime CreatedOn { get; set; }
}
