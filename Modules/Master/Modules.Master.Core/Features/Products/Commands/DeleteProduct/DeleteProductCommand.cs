using MediatR;
using Shared.Core.Wrapper;

namespace Modules.Master.Core.Features.Products.Commands.DeleteProduct;

public record DeleteProductCommand(int Id) : IRequest<IResult>;
