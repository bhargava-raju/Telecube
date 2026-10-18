namespace Search.Api.Contracts;

public record ProductSearchResponse(IEnumerable<SearchProductResponse> Items, long TotalCount, int Page, int PageSize);
