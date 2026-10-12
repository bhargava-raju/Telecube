namespace Catalog.Api.Contracts;

public record CreateProductRequest(Guid CategoryId, string Sku, string Name, string Description, decimal Price);
