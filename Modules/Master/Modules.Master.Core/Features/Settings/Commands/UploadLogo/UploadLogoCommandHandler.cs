using MediatR;
using Modules.Master.Core.Abstractions;
using Shared.Core.Abstractions;
using Shared.Core.Wrapper;

namespace Modules.Master.Core.Features.Settings.Commands.UploadLogo;

public class UploadLogoCommandHandler : IRequestHandler<UploadLogoCommand, Result<string>>
{
    private const string Folder = "logo";

    private readonly IFileStorageService _fileStorageService;
    private readonly ISettingsRepository _settingsRepository;

    public UploadLogoCommandHandler(IFileStorageService fileStorageService, ISettingsRepository settingsRepository)
    {
        _fileStorageService = fileStorageService;
        _settingsRepository = settingsRepository;
    }

    public async Task<Result<string>> Handle(UploadLogoCommand request, CancellationToken cancellationToken)
    {
        var url = await _fileStorageService.UploadImageAsync(request.FileStream, request.FileName, Folder, cancellationToken);

        var settings = await _settingsRepository.GetSettingsAsync(cancellationToken);
        var isNew = settings is null;
        settings ??= new Entities.Settings();

        settings.LogoUrl = url;

        if (isNew)
        {
            await _settingsRepository.AddAsync(settings, cancellationToken);
        }
        else
        {
            _settingsRepository.Update(settings);
        }

        await _settingsRepository.SaveChangesAsync(cancellationToken);

        return Result<string>.Success(url, "Logo uploaded successfully.");
    }
}
