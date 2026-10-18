namespace Search.Api.Contracts;

public record SearchProductResponse(Guid Id, Guid CategoryId, string Sku, string Name, string Description, decimal Price);
