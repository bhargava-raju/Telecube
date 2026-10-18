using Search.Domain;

namespace Search.Application.Abstractions;

public interface IProductSearchIndex
{
    Task UpsertAsync(ProductDocument document, CancellationToken cancellationToken);
    Task DeleteAsync(Guid productId, CancellationToken cancellationToken);
    Task<ProductSearchResult> SearchAsync(ProductSearchCriteria criteria, CancellationToken cancellationToken);
}
