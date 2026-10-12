using Catalog.Application.Abstractions;
using Catalog.Domain;
using MediatR;
using Telecube.BuildingBlocks.Common;

namespace Catalog.Application.Queries.GetCategoryById;

public class GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository)
    : IRequestHandler<GetCategoryByIdQuery, Result<Category>>
{
    public async Task<Result<Category>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(request.Id, cancellationToken);
        if (category == null)
        {
            return Result<Category>.Failure(new Error("NotFound", $"Category with id {request.Id} not found", ErrorType.NotFound));
        }

        return Result<Category>.Success(category);
    }
}
