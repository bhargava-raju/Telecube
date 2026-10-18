using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using Search.Application.Abstractions;
using Search.Domain;

namespace Search.Infrastructure.Indexing;

public sealed class AzureProductSearchIndex(SearchClient searchClient) : IProductSearchIndex
{
    public async Task UpsertAsync(ProductDocument document, CancellationToken cancellationToken)
    {
        // Upload replaces the whole document if one with the same key exists.
        await searchClient.UploadDocumentsAsync([ToIndexDocument(document)], cancellationToken: cancellationToken);
    }

    public async Task DeleteAsync(Guid productId, CancellationToken cancellationToken)
    {
        await searchClient.DeleteDocumentsAsync(
            nameof(ProductIndexDocument.Id), [productId.ToString()], cancellationToken: cancellationToken);
    }

    public async Task<ProductSearchResult> SearchAsync(ProductSearchCriteria criteria, CancellationToken cancellationToken)
    {
        var options = new SearchOptions
        {
            Filter = BuildFilter(criteria),
            Size = criteria.PageSize,
            Skip = (criteria.Page - 1) * criteria.PageSize,
            IncludeTotalCount = true
        };

        // With no search text there is no relevance score to rank by, so list alphabetically.
        if (criteria.Text is null)
            options.OrderBy.Add($"{nameof(ProductIndexDocument.Name)} asc");

        var response = await searchClient.SearchAsync<ProductIndexDocument>(criteria.Text ?? "*", options, cancellationToken);

        var items = new List<ProductDocument>();
        await foreach (var result in response.Value.GetResultsAsync().WithCancellation(cancellationToken))
        {
            items.Add(ToDomain(result.Document));
        }

        return new ProductSearchResult(items, response.Value.TotalCount ?? 0);
    }

    // SearchFilter.Create quotes and escapes each value, so user input can't break out of the OData filter.
    private static string? BuildFilter(ProductSearchCriteria criteria)
    {
        var clauses = new List<string>();

        if (criteria.CategoryId is { } categoryId)
            clauses.Add(SearchFilter.Create($"{nameof(ProductIndexDocument.CategoryId)} eq {categoryId.ToString()}"));

        if (criteria.MinPrice is { } minPrice)
            clauses.Add(SearchFilter.Create($"{nameof(ProductIndexDocument.Price)} ge {(double)minPrice}"));

        if (criteria.MaxPrice is { } maxPrice)
            clauses.Add(SearchFilter.Create($"{nameof(ProductIndexDocument.Price)} le {(double)maxPrice}"));

        return clauses.Count == 0 ? null : string.Join(" and ", clauses);
    }

    private static ProductIndexDocument ToIndexDocument(ProductDocument document) => new()
    {
        Id = document.Id.ToString(),
        CategoryId = document.CategoryId.ToString(),
        Sku = document.Sku,
        Name = document.Name,
        Description = document.Description,
        Price = (double)document.Price
    };

    // Index content was validated before it was written, so a failure here means the index is corrupt.
    private static ProductDocument ToDomain(ProductIndexDocument document)
    {
        var result = ProductDocument.Create(
            Guid.Parse(document.Id),
            Guid.Parse(document.CategoryId),
            document.Sku,
            document.Name,
            document.Description,
            (decimal)document.Price);

        return result.IsSuccess
            ? result.Value
            : throw new InvalidOperationException($"Search document {document.Id} is invalid: {result.Error.Message}");
    }
}
