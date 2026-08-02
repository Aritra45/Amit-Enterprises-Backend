using AutoMapper;
using MediatR;
using Modules.Master.Core.Abstractions;
using Shared.Core.Wrapper;

namespace Modules.Master.Core.Features.Settings.Queries.GetSettings;

public class GetSettingsQueryHandler : IRequestHandler<GetSettingsQuery, Result<SettingsResponse>>
{
    private readonly ISettingsRepository _settingsRepository;
    private readonly IMapper _mapper;

    public GetSettingsQueryHandler(ISettingsRepository settingsRepository, IMapper mapper)
    {
        _settingsRepository = settingsRepository;
        _mapper = mapper;
    }

    public async Task<Result<SettingsResponse>> Handle(GetSettingsQuery request, CancellationToken cancellationToken)
    {
        var settings = await _settingsRepository.GetSettingsAsync(cancellationToken);

        return settings is null
            ? Result<SettingsResponse>.Success(new SettingsResponse())
            : Result<SettingsResponse>.Success(_mapper.Map<SettingsResponse>(settings));
    }
}
