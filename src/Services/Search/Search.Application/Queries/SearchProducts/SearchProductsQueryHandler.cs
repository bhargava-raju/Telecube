using MediatR;
using Search.Application.Abstractions;
using Search.Domain;
using Telecube.BuildingBlocks.Common;

namespace Search.Application.Queries.SearchProducts;

public class SearchProductsQueryHandler(IProductSearchIndex searchIndex)
    : IRequestHandler<SearchProductsQuery, Result<ProductSearchResult>>
{
    public async Task<Result<ProductSearchResult>> Handle(SearchProductsQuery request, CancellationToken cancellationToken)
    {
        var criteria = ProductSearchCriteria.Create(request.Text, request.CategoryId, request.MinPrice, request.MaxPrice, request.Page, request.PageSize);
        if(criteria.IsSuccess)
        {
            var searchResult = await searchIndex.SearchAsync(criteria.Value, cancellationToken);
            return Result<ProductSearchResult>.Success(searchResult);
        }
        return Result<ProductSearchResult>.Failure(Error.None);
    }
}
