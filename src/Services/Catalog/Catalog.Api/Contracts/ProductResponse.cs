namespace Catalog.Api.Contracts;

public record ProductResponse(Guid Id, Guid CategoryId, string Sku, string Name, string Description, decimal Price);
