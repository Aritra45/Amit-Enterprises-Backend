using AutoMapper;
using MediatR;
using Modules.Master.Core.Abstractions;
using Shared.Core.Wrapper;

namespace Modules.Master.Core.Features.Settings.Commands.UpdateSettings;

public class UpdateSettingsCommandHandler : IRequestHandler<UpdateSettingsCommand, Result<SettingsResponse>>
{
    private readonly ISettingsRepository _settingsRepository;
    private readonly IMapper _mapper;

    public UpdateSettingsCommandHandler(ISettingsRepository settingsRepository, IMapper mapper)
    {
        _settingsRepository = settingsRepository;
        _mapper = mapper;
    }

    public async Task<Result<SettingsResponse>> Handle(UpdateSettingsCommand request, CancellationToken cancellationToken)
    {
        var settings = await _settingsRepository.GetSettingsAsync(cancellationToken);
        var isNew = settings is null;
        settings ??= new Entities.Settings();

        settings.ShopName = request.ShopName;
        settings.OwnerName = request.OwnerName;
        settings.MobileNumber = request.MobileNumber;
        settings.Address = request.Address;
        settings.GSTNumber = request.GSTNumber;
        settings.LogoUrl = request.LogoUrl;

        if (isNew)
        {
            await _settingsRepository.AddAsync(settings, cancellationToken);
        }
        else
        {
            _settingsRepository.Update(settings);
        }

        await _settingsRepository.SaveChangesAsync(cancellationToken);

        return Result<SettingsResponse>.Success(_mapper.Map<SettingsResponse>(settings), "Settings updated successfully.");
    }
}
