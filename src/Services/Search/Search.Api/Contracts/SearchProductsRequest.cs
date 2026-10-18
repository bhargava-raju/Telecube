namespace Search.Api.Contracts;

// Bound from the query string, e.g. /api/search?q=phone&minPrice=100&page=2
public record SearchProductsRequest
{
    public string? Q { get; init; }
    public Guid? CategoryId { get; init; }
    public decimal? MinPrice { get; init; }
    public decimal? MaxPrice { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}
