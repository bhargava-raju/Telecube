namespace Cart.Application.Abstractions;

public interface ICatalogClient
{
    Task<CatalogProduct?> GetProductAsync(Guid productId, CancellationToken cancellationToken);
}
