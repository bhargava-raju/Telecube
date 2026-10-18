namespace Cart.Api.Contracts;

public record CartResponse(IEnumerable<CartItemResponse> Items, decimal Total);
