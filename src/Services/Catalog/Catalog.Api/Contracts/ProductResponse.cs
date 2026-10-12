namespace Catalog.Api.Contracts;

public record ProductResponse(Guid Id, string Sku, string Name, string Description, decimal Price);
