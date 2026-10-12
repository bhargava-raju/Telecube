using Catalog.Application.Abstractions;
using MediatR;
using Telecube.BuildingBlocks.Common;

namespace Catalog.Application.Commands.DeleteCategory;

public class DeleteCategoryCommandHandler(ICategoryRepository categoryRepository)
    : IRequestHandler<DeleteCategoryCommand, Result>
{
    public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(request.Id, cancellationToken);
        if (category == null)
        {
            return Result.Failure(new Error("NotFound", $"Category with id {request.Id} not found", ErrorType.NotFound));
        }

        if (await categoryRepository.HasProductsAsync(category.Id, cancellationToken))
        {
            return Result.Failure(new Error("Conflict", $"Category with id {request.Id} still has products", ErrorType.Conflict));
        }

        await categoryRepository.DeleteAsync(category.Id, cancellationToken);
        return Result.Success();
    }
}
