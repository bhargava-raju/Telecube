using Telecube.BuildingBlocks.Common;

namespace Search.Domain;

// What the caller is searching for: free text plus optional category and price filters, one page at a time.
public sealed class ProductSearchCriteria
{
    public const int MaxPageSize = 50;

    public string? Text { get; }
    public Guid? CategoryId { get; }
    public decimal? MinPrice { get; }
    public decimal? MaxPrice { get; }
    public int Page { get; }
    public int PageSize { get; }

    private ProductSearchCriteria(string? text, Guid? categoryId, decimal? minPrice, decimal? maxPrice, int page, int pageSize)
    {
        Text = text;
        CategoryId = categoryId;
        MinPrice = minPrice;
        MaxPrice = maxPrice;
        Page = page;
        PageSize = pageSize;
    }

    public static Result<ProductSearchCriteria> Create(string? text, Guid? categoryId, decimal? minPrice, decimal? maxPrice, int page, int pageSize)
    {
        if (page < 1)
            return Result<ProductSearchCriteria>.Failure(Error.Validation("Validation.Page", $"Page must be 1 or more. Parameter name: {nameof(page)}"));

        if (pageSize < 1 || pageSize > MaxPageSize)
            return Result<ProductSearchCriteria>.Failure(Error.Validation("Validation.PageSize", $"Page size must be between 1 and {MaxPageSize}. Parameter name: {nameof(pageSize)}"));

        if (minPrice < 0)
            return Result<ProductSearchCriteria>.Failure(Error.Validation("Validation.Price", $"Price cannot be negative. Parameter name: {nameof(minPrice)}"));

        if (maxPrice < 0)
            return Result<ProductSearchCriteria>.Failure(Error.Validation("Validation.Price", $"Price cannot be negative. Parameter name: {nameof(maxPrice)}"));

        if (minPrice > maxPrice)
            return Result<ProductSearchCriteria>.Failure(Error.Validation("Validation.PriceRange", $"Minimum price cannot be above maximum price. Parameter name: {nameof(minPrice)}"));

        // A blank search box means "match everything", not "match the empty string".
        var trimmedText = string.IsNullOrWhiteSpace(text) ? null : text.Trim();

        return Result<ProductSearchCriteria>.Success(
            new ProductSearchCriteria(trimmedText, categoryId, minPrice, maxPrice, page, pageSize));
    }
}
