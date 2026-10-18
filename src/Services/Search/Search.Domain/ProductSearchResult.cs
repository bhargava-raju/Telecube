namespace Search.Domain;

// One page of matches plus the total number of matches across all pages.
public sealed record ProductSearchResult(IReadOnlyList<ProductDocument> Items, long TotalCount);
