using MediatR;
using Shared.Core.Wrapper;

namespace Modules.Master.Core.Features.Categories.Queries.GetCategoryById;

public record GetCategoryByIdQuery(int Id) : IRequest<Result<CategoryResponse>>;
