namespace Shared.DTOs.Pagination;

/// <summary>
/// Base class for query/list requests. Bind the whole request object from the query string
/// (e.g. [FromQuery] GetProductsQuery request) instead of separate pageNumber/pageSize action parameters.
/// Pass PageSize = -1 to receive the full, unpaginated result set.
/// </summary>
public abstract class PaginationRequest
{
    private int _pageNumber = 1;
    private int _pageSize = 10;

    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value <= 0 ? 1 : value;
    }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value == -1 ? -1 : (value <= 0 ? 10 : value);
    }

    public string? SearchTerm { get; set; }

    public string? SortColumn { get; set; }

    public bool SortDescending { get; set; }
}
