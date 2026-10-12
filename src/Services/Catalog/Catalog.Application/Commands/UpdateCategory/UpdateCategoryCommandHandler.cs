using Catalog.Application.Abstractions;
using MediatR;
using Telecube.BuildingBlocks.Common;

namespace Catalog.Application.Commands.UpdateCategory;

public class UpdateCategoryCommandHandler(ICategoryRepository categoryRepository)
    : IRequestHandler<UpdateCategoryCommand, Result>
{
    public async Task<Result> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(request.Id, cancellationToken);
        if (category == null)
        {
            return Result.Failure(new Error("NotFound", $"Category with id {request.Id} not found", ErrorType.NotFound));
        }

        var categoryWithSameName = await categoryRepository.GetByNameAsync(request.Name, cancellationToken);
        if (categoryWithSameName != null && categoryWithSameName.Id != category.Id)
        {
            return Result.Failure(new Error("Conflict", $"Category with name {request.Name} already exists", ErrorType.Conflict));
        }

        var result = category.Update(request.Name, request.Description);
        if (result.IsFailure)
        {
            return result;
        }

        await categoryRepository.UpdateAsync(category, cancellationToken);
        return Result.Success();
    }
}
