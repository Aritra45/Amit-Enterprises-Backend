using MediatR;
using Shared.Core.Wrapper;
using Shared.DTOs.Pagination;

namespace Modules.Master.Core.Features.Products.Queries.GetProducts;

public class GetProductsQuery : PaginationRequest, IRequest<PaginatedResult<ProductResponse>>
{
    public int? CategoryId { get; set; }

    public bool? LowStockOnly { get; set; }
}
