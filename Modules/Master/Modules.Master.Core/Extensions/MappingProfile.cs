using AutoMapper;
using Modules.Master.Core.Features.Categories;
using Modules.Master.Core.Features.Products;

namespace Modules.Master.Core.Extensions;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Entities.Category, CategoryResponse>();

        CreateMap<Entities.Product, ProductResponse>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName));

        CreateMap<Entities.Settings, Features.Settings.SettingsResponse>();
    }
}
