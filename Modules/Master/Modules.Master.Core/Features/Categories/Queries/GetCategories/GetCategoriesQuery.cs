using MediatR;
using Shared.Core.Wrapper;
using Shared.DTOs.Pagination;

namespace Modules.Master.Core.Features.Categories.Queries.GetCategories;

public class GetCategoriesQuery : PaginationRequest, IRequest<PaginatedResult<CategoryResponse>>
{
}
