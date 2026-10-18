namespace Cart.Api.Contracts;

public record AddCartItemRequest(Guid ProductId, int Quantity);
