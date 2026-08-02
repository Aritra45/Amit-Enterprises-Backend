using AutoMapper;
using Modules.Booking.Core.Features.Expenses;

namespace Modules.Booking.Core.Extensions;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Entities.Expense, ExpenseResponse>();
    }
}
