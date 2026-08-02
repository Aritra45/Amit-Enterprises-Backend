using MediatR;
using Modules.Master.Core.Abstractions;
using Shared.Core.Exceptions;
using Shared.Core.Wrapper;

namespace Modules.Master.Core.Features.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, IResult>
{
    private readonly ICategoryRepository _categoryRepository;

    public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IResult> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Category", request.Id);

        if (await _categoryRepository.HasProductsAsync(request.Id, cancellationToken))
        {
            throw new ConflictException("Cannot delete a category that still has products assigned to it.");
        }

        _categoryRepository.Remove(category);
        await _categoryRepository.SaveChangesAsync(cancellationToken);

        return Result.Success("Category deleted successfully.");
    }
}
