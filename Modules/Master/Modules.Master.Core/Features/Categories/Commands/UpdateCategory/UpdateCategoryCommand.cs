using MediatR;
using Shared.Core.Wrapper;

namespace Modules.Master.Core.Features.Categories.Commands.UpdateCategory;

public record UpdateCategoryCommand(int Id, string CategoryName, string? Description) : IRequest<IResult>;
