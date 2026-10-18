namespace Telecube.Contracts;

public sealed record CheckoutRequestedItem(Guid ProductId, string Sku, string Name, decimal UnitPrice, int Quantity);
