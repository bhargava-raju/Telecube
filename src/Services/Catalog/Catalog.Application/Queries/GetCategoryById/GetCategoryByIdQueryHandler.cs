using Catalog.Application.Abstractions;
using Catalog.Domain;
using MediatR;
using Telecube.BuildingBlocks.Common;

namespace Catalog.Application.Queries.GetCategoryById;

public class GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository)
    : IRequestHandler<GetCategoryByIdQuery, Result<Category>>
{
    // TODO: Return the category with the given id, or not-found if there is none.
    public Task<Result<Category>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
