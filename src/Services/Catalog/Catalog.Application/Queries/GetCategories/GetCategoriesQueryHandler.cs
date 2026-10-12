using Catalog.Application.Abstractions;
using Catalog.Domain;
using MediatR;

namespace Catalog.Application.Queries.GetCategories;

public class GetCategoriesQueryHandler(ICategoryRepository categoryRepository)
    : IRequestHandler<GetCategoriesQuery, IReadOnlyList<Category>>
{
    // TODO: Return every category in the catalog.
    public Task<IReadOnlyList<Category>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
