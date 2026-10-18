namespace Cart.Api.Contracts;

public record CartItemResponse(Guid ProductId, string Sku, string Name, decimal UnitPrice, int Quantity, decimal LineTotal);
