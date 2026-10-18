namespace Cart.Application.Abstractions;

// The product fields Cart needs from Catalog, independent of Catalog's own API shape.
public record CatalogProduct(Guid Id, string Sku, string Name, decimal Price);
