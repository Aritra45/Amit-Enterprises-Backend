using System;
using System.Collections.Generic;

namespace Shared.Core.Wrapper;

public class PaginatedResult<T> : Result
{
    public List<T>? Data { get; set; }

    public int CurrentPage { get; set; }

    public int TotalPages { get; set; }

    public int TotalCount { get; set; }

    public int PageSize { get; set; }

    public bool HasPreviousPage => CurrentPage > 1;

    public bool HasNextPage => CurrentPage < TotalPages;

    public double? TotalPaidAmount { get; set; }

    public static PaginatedResult<T> Failure(string message)
    {
        return new()
        {
            Succeeded = false,
            Messages = new List<string> { message }
        };
    }

    public static PaginatedResult<T> Failure(List<string> messages)
    {
        return new()
        {
            Succeeded = false,
            Messages = messages
        };
    }

    public static PaginatedResult<T> Success(List<T> data, int count, int page, int pageSize)
    {
        // pageSize == -1 signals "return everything" - the whole result set is a single page.
        if (pageSize == -1)
        {
            return new()
            {
                Succeeded = true,
                Data = data,
                CurrentPage = 1,
                PageSize = count,
                TotalPages = count == 0 ? 0 : 1,
                TotalCount = count
            };
        }

        return new()
        {
            Succeeded = true,
            Data = data,
            CurrentPage = page,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling(count / (double)pageSize),
            TotalCount = count
        };
    }

    public static PaginatedResult<T> Success(List<T> data, int count, int page, int pageSize, double totalPaidAmount)
    {
        var result = Success(data, count, page, pageSize);
        result.TotalPaidAmount = totalPaidAmount;
        return result;
    }
}
