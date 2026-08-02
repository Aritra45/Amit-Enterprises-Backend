namespace Shared.Core.Entities;

public abstract class BaseEntity
{
    public int Id { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }
    public bool IsActive { get; set; } = true;

    public bool IsDeleted { get; set; }
}
