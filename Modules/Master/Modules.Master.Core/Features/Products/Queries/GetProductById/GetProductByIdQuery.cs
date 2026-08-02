using MediatR;
using Shared.Core.Wrapper;

namespace Modules.Master.Core.Features.Products.Queries.GetProductById;

public record GetProductByIdQuery(int Id) : IRequest<Result<ProductResponse>>;
