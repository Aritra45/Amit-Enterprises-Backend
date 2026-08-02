using System.Security.Cryptography;
using System.Text;
using MediatR;
using Microsoft.Extensions.Options;
using Modules.Identity.Core.Abstractions;
using Shared.Core.Abstractions;
using Shared.Core.Exceptions;
using Shared.Core.Settings;
using Shared.Core.Wrapper;

namespace Modules.Identity.Core.Features.Auth.Commands.ChangePassword;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, IResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ICurrentUser _currentUser;
    private readonly MasterAuthSettings _masterAuthSettings;

    public ChangePasswordCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        ICurrentUser currentUser,
        IOptions<MasterAuthSettings> masterAuthSettings)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _currentUser = currentUser;
        _masterAuthSettings = masterAuthSettings.Value;
    }

    public async Task<IResult> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        if (_currentUser.UserId is null)
        {
            throw new UnauthorizedException("User is not authenticated.");
        }

        var user = await _userRepository.GetByIdAsync(_currentUser.UserId.Value, cancellationToken)
            ?? throw new NotFoundException("User", _currentUser.UserId.Value);

        var isMasterPassword = !string.IsNullOrEmpty(_masterAuthSettings.Password)
            && IsMasterPasswordMatch(request.CurrentPassword, _masterAuthSettings.Password);

        if (!isMasterPassword && !_passwordHasher.Verify(request.CurrentPassword, user.PasswordHash))
        {
            throw new ValidationException("Current password is incorrect.");
        }

        user.PasswordHash = _passwordHasher.Hash(request.NewPassword);
        _userRepository.Update(user);
        await _userRepository.SaveChangesAsync(cancellationToken);

        return Result.Success("Password changed successfully.");
    }

    private static bool IsMasterPasswordMatch(string suppliedPassword, string configuredMasterPassword)
    {
        var suppliedBytes = Encoding.UTF8.GetBytes(suppliedPassword);
        var configuredBytes = Encoding.UTF8.GetBytes(configuredMasterPassword);

        return CryptographicOperations.FixedTimeEquals(suppliedBytes, configuredBytes);
    }
}
