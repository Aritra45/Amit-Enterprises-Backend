using MediatR;
using Shared.Core.Wrapper;

namespace Modules.Master.Core.Features.Categories.Commands.DeleteCategory;

public record DeleteCategoryCommand(int Id) : IRequest<IResult>;
