using System.Security.Cryptography;
using MediatR;
using Microsoft.Extensions.Logging;
using Modules.Identity.Core.Abstractions;
using Shared.Core.Abstractions;
using Shared.Core.Wrapper;

namespace Modules.Identity.Core.Features.Auth.Commands.ForgotPassword;

public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, IResult>
{
    private static readonly char[] OtpChars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789".ToCharArray();
    private static readonly TimeSpan OtpValidity = TimeSpan.FromMinutes(10);

    private readonly IUserRepository _userRepository;
    private readonly IPasswordResetOtpRepository _otpRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IEmailService _emailService;
    private readonly ILogger<ForgotPasswordCommandHandler> _logger;

    public ForgotPasswordCommandHandler(
        IUserRepository userRepository,
        IPasswordResetOtpRepository otpRepository,
        IPasswordHasher passwordHasher,
        IEmailService emailService,
        ILogger<ForgotPasswordCommandHandler> logger)
    {
        _userRepository = userRepository;
        _otpRepository = otpRepository;
        _passwordHasher = passwordHasher;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<IResult> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

        // Always return the same response whether or not the email is registered
        // (and even if the email fails to send), so this endpoint can't be used
        // to enumerate valid accounts or distinguish SMTP failures.
        if (user is not null && user.IsActive)
        {
            var otpCode = GenerateOtp();

            await _otpRepository.InvalidatePendingForUserAsync(user.Id, cancellationToken);

            await _otpRepository.AddAsync(new Entities.PasswordResetOtp
            {
                UserId = user.Id,
                OtpHash = _passwordHasher.Hash(otpCode),
                ExpiresOn = DateTime.UtcNow.Add(OtpValidity)
            }, cancellationToken);

            await _otpRepository.SaveChangesAsync(cancellationToken);

            try
            {
                await _emailService.SendAsync(
                    user.Email,
                    "Your Amit Enterprises password reset code",
                    $"<p>Your password reset code is:</p><h2 style=\"letter-spacing:4px\">{otpCode}</h2><p>This code expires in 10 minutes. If you didn't request this, you can ignore this email.</p>",
                    cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send password reset email to user {UserId}.", user.Id);
            }
        }

        return Result.Success("If this email is registered, a verification code has been sent.");
    }

    private static string GenerateOtp()
    {
        Span<char> buffer = stackalloc char[6];
        for (var i = 0; i < buffer.Length; i++)
        {
            buffer[i] = OtpChars[RandomNumberGenerator.GetInt32(OtpChars.Length)];
        }

        return new string(buffer);
    }
}
