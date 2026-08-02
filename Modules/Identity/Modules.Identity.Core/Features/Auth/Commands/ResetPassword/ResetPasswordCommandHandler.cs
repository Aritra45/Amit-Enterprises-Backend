using MediatR;
using Modules.Identity.Core.Abstractions;
using Shared.Core.Abstractions;
using Shared.Core.Exceptions;
using Shared.Core.Wrapper;

namespace Modules.Identity.Core.Features.Auth.Commands.ResetPassword;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, IResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordResetOtpRepository _otpRepository;
    private readonly IPasswordHasher _passwordHasher;

    public ResetPasswordCommandHandler(
        IUserRepository userRepository,
        IPasswordResetOtpRepository otpRepository,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _otpRepository = otpRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<IResult> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        var otp = user is null ? null : await _otpRepository.GetLatestPendingForUserAsync(user.Id, cancellationToken);

        if (otp is null || otp.IsExpired || !otp.IsVerified || !_passwordHasher.Verify(request.Otp, otp.OtpHash))
        {
            throw new ValidationException("Invalid or expired code.");
        }

        user!.PasswordHash = _passwordHasher.Hash(request.NewPassword);
        _userRepository.Update(user);

        otp.IsUsed = true;
        _otpRepository.Update(otp);

        await _userRepository.SaveChangesAsync(cancellationToken);

        return Result.Success("Password reset successfully.");
    }
}
