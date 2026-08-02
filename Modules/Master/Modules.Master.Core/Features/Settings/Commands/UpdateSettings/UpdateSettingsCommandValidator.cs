using FluentValidation;

namespace Modules.Master.Core.Features.Settings.Commands.UpdateSettings;

public class UpdateSettingsCommandValidator : AbstractValidator<UpdateSettingsCommand>
{
    public UpdateSettingsCommandValidator()
    {
        RuleFor(x => x.ShopName)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.OwnerName)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.MobileNumber)
            .Matches(@"^\d{10}$")
            .When(x => !string.IsNullOrWhiteSpace(x.MobileNumber))
            .WithMessage("Mobile number must be exactly 10 digits.");

        RuleFor(x => x.GSTNumber)
            .MaximumLength(15);
    }
}
