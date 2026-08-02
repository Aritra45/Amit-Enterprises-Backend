namespace Shared.Core.Exceptions;

public class ValidationException : ApiException
{
    public ValidationException()
        : base("One or more validation failures have occurred.", 400)
    {
    }

    public ValidationException(List<string> errors)
        : base(errors, 400)
    {
    }

    public ValidationException(string message)
        : base(message, 400)
    {
    }
}
