using MediatR;
using Shared.Core.Wrapper;

namespace Modules.Master.Core.Features.Categories.Commands.CreateCategory;

public record CreateCategoryCommand(string CategoryName, string? Description) : IRequest<Result<int>>;
