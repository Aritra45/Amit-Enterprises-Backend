using AutoMapper;
using MediatR;
using Modules.Master.Core.Abstractions;
using Shared.Core.Wrapper;

namespace Modules.Master.Core.Features.Products.Queries.GetProducts;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, PaginatedResult<ProductResponse>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetProductsQueryHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedResult<ProductResponse>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _productRepository.GetPagedWithCategoryAsync(
            request.CategoryId,
            request.LowStockOnly,
            request.SearchTerm,
            request.SortColumn,
            request.SortDescending,
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        var mapped = _mapper.Map<List<ProductResponse>>(items);

        return PaginatedResult<ProductResponse>.Success(mapped, totalCount, request.PageNumber, request.PageSize);
    }
}
