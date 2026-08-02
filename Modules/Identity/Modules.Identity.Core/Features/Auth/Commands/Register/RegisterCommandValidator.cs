using FluentValidation;

namespace Modules.Identity.Core.Features.Auth.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(150);

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6);

        RuleFor(x => x.MobileNumber)
            .Matches(@"^\d{10}$")
            .When(x => !string.IsNullOrWhiteSpace(x.MobileNumber))
            .WithMessage("Mobile number must be exactly 10 digits.");

        RuleFor(x => x.RoleId)
            .GreaterThan(0);
    }
}
