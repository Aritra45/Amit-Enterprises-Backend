namespace Shared.Core.Exceptions;

public class ApiException : Exception
{
    public List<string> Errors { get; }

    public int StatusCode { get; }

    public ApiException(string message, int statusCode = 500)
        : base(message)
    {
        Errors = new List<string> { message };
        StatusCode = statusCode;
    }

    public ApiException(List<string> errors, int statusCode = 500)
        : base(errors.FirstOrDefault() ?? "An error occurred")
    {
        Errors = errors;
        StatusCode = statusCode;
    }
}
