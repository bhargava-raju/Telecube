using MediatR;
using Search.Application.Abstractions;
using Search.Domain;
using Telecube.BuildingBlocks.Common;

namespace Search.Application.Queries.SearchProducts;

public class SearchProductsQueryHandler(IProductSearchIndex searchIndex)
    : IRequestHandler<SearchProductsQuery, Result<ProductSearchResult>>
{
    // TODO: Build search criteria from the query and return the matching page from the index, or the validation failure.
    public Task<Result<ProductSearchResult>> Handle(SearchProductsQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
