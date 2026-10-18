using MediatR;
using Search.Domain;
using Telecube.BuildingBlocks.Common;

namespace Search.Application.Queries.SearchProducts;

public record SearchProductsQuery(string? Text, Guid? CategoryId, decimal? MinPrice, decimal? MaxPrice, int Page, int PageSize)
    : IRequest<Result<ProductSearchResult>>;
