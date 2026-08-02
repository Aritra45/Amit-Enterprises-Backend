namespace Shared.Core.Constants;

public static class AppConstants
{
    public const int DefaultPageNumber = 1;

    public const int DefaultPageSize = 10;

    /// <summary>Pass this as PageSize to signal "return the full result set, unpaginated".</summary>
    public const int UnpagedPageSize = -1;

    public const int MaxPageSize = 200;
}
