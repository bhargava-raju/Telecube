using Catalog.Application.Abstractions;
using Catalog.Domain;
using MediatR;
using Telecube.BuildingBlocks.Common;

namespace Catalog.Application.Queries.GetCategories;

public class GetCategoriesQueryHandler(ICategoryRepository categoryRepository)
    : IRequestHandler<GetCategoriesQuery, Result<IReadOnlyList<Category>>>
{
    // TODO: Return every category in the catalog.
    public async Task<Result<IReadOnlyList<Category>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await categoryRepository.GetAllAsync(cancellationToken);
        return Result<IReadOnlyList<Category>>.Success(categories);
    }
}
