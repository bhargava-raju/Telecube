using Catalog.Application.Abstractions;
using Catalog.Domain;
using MediatR;
using Telecube.BuildingBlocks.Common;

namespace Catalog.Application.Commands.CreateCategory;

public class CreateCategoryCommandHandler(ICategoryRepository categoryRepository)
    : IRequestHandler<CreateCategoryCommand, Result<Category>>
{
    public async Task<Result<Category>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var result = Category.Create(request.Name, request.Description);
        if (result.IsFailure)
        {
            return Result<Category>.Failure(result.Error);
        }

        var existingCategory = await categoryRepository.GetByNameAsync(request.Name, cancellationToken);
        if (existingCategory != null)
        {
            return Result<Category>.Failure(new Error("Conflict", $"Category with name {request.Name} already exists", ErrorType.Conflict));
        }

        await categoryRepository.AddAsync(result.Value, cancellationToken);
        return Result<Category>.Success(result.Value);
    }
}
