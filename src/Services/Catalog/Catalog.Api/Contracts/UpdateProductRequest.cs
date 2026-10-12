namespace Catalog.Api.Contracts;

public record UpdateProductRequest(Guid CategoryId, string Name, string Description, decimal Price);
