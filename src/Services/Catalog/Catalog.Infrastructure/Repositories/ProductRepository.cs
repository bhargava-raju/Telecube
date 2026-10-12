using Catalog.Application.Abstractions;
using Catalog.Domain;

namespace Catalog.Infrastructure.Repositories;

public sealed class ProductRepository(ISqlConnectionFactory connectionFactory) : IProductRepository
{
    // TODO: Load the product row with the given id, or null if none exists.
    public Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    // TODO: Load the product row with the given SKU, or null if none exists.
    public Task<Product?> GetBySkuAsync(string sku, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    // TODO: Load every product row.
    public Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    // TODO: Insert a new row for the product.
    public Task AddAsync(Product product, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    // TODO: Save the product's category, name, description and price to its existing row.
    public Task UpdateAsync(Product product, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    // TODO: Remove the row with the given id.
    public Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
