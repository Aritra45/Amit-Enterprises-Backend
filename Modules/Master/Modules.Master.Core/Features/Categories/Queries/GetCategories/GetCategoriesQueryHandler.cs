using AutoMapper;
using MediatR;
using Modules.Master.Core.Abstractions;
using Shared.Core.Wrapper;

namespace Modules.Master.Core.Features.Categories.Queries.GetCategories;

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, PaginatedResult<CategoryResponse>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public GetCategoriesQueryHandler(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedResult<CategoryResponse>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var search = request.SearchTerm?.Trim();

        var (items, totalCount) = await _categoryRepository.GetPagedAsync(
            filter: search == null
                ? null
                : c => c.CategoryName.Contains(search) || (c.Description != null && c.Description.Contains(search)),
            orderBy: query => request.SortColumn?.ToLower() switch
            {
                "categoryname" => request.SortDescending ? query.OrderByDescending(c => c.CategoryName) : query.OrderBy(c => c.CategoryName),
                _ => request.SortDescending ? query.OrderByDescending(c => c.CreatedOn) : query.OrderBy(c => c.CreatedOn)
            },
            pageNumber: request.PageNumber,
            pageSize: request.PageSize,
            cancellationToken: cancellationToken);

        var mapped = _mapper.Map<List<CategoryResponse>>(items);

        return PaginatedResult<CategoryResponse>.Success(mapped, totalCount, request.PageNumber, request.PageSize);
    }
}
