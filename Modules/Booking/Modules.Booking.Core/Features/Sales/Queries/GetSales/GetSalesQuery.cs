using MediatR;
using Shared.Core.Wrapper;
using Shared.DTOs.Pagination;

namespace Modules.Booking.Core.Features.Sales.Queries.GetSales;

/// <summary>SearchTerm (inherited) is matched against the invoice number.</summary>
public class GetSalesQuery : PaginationRequest, IRequest<PaginatedResult<SaleResponse>>
{
    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }
}
