using MediatR;
using Shared.Core.Wrapper;

namespace Modules.Booking.Core.Features.Sales.Queries.GetSaleById;

public record GetSaleByIdQuery(int Id) : IRequest<Result<SaleResponse>>;
