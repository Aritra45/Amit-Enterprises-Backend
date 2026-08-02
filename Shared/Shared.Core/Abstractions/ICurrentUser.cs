namespace Shared.Core.Abstractions;

public interface ICurrentUser
{
    int? UserId { get; }

    string? UserName { get; }

    string? Role { get; }

    bool IsAuthenticated { get; }
}
