using AutoMapper;
using Modules.Identity.Core.Entities;
using Modules.Identity.Core.Features.Auth.Queries.GetProfile;

namespace Modules.Identity.Core.Extensions;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, ProfileResponse>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.Name));
    }
}
