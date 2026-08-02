using FluentValidation;

namespace Modules.Master.Core.Features.Settings.Commands.UploadLogo;

public class UploadLogoCommandValidator : AbstractValidator<UploadLogoCommand>
{
    public UploadLogoCommandValidator()
    {
        RuleFor(x => x.FileName).NotEmpty();
        RuleFor(x => x.FileStream).NotNull();
    }
}
