using MediatR;
using Modules.Master.Core.Abstractions;
using Shared.Core.Exceptions;
using Shared.Core.Wrapper;

namespace Modules.Master.Core.Features.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, IResult>
{
    private readonly ICategoryRepository _categoryRepository;

    public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IResult> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Category", request.Id);

        if (await _categoryRepository.NameExistsAsync(request.CategoryName, request.Id, cancellationToken))
        {
            throw new ConflictException($"A category named '{request.CategoryName}' already exists.");
        }

        category.CategoryName = request.CategoryName;
        category.Description = request.Description;

        _categoryRepository.Update(category);
        await _categoryRepository.SaveChangesAsync(cancellationToken);

        return Result.Success("Category updated successfully.");
    }
}
