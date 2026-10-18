using Telecube.BuildingBlocks.Common;

namespace Cart.Domain;

// One cart per shopper. The cart's Id is the shopper's Entra object id (the token's oid claim).
public class Cart : AggregateRoot
{
    public const int MaxQuantityPerItem = 99;

    private readonly List<CartItem> _items = [];

    public Guid CustomerId => Id;
    public IReadOnlyList<CartItem> Items => _items.AsReadOnly();
    public decimal Total => _items.Sum(item => item.LineTotal);
    public bool IsEmpty => _items.Count == 0;

    private Cart() { }

    public static Result<Cart> Create(Guid customerId)
    {
        if (customerId == Guid.Empty)
            return Result<Cart>.Failure(Error.Validation("Validation.Required", $"Value cannot be empty. Parameter name: {nameof(customerId)}"));

        return Result<Cart>.Success(new Cart { Id = customerId });
    }

    // Adding a product that's already in the cart increases its quantity and refreshes its snapshot to the latest catalog values.
    public Result AddItem(Guid productId, string sku, string name, decimal unitPrice, int quantity)
    {
        if (productId == Guid.Empty)
            return Result.Failure(Error.Validation("Validation.Required", $"Value cannot be empty. Parameter name: {nameof(productId)}"));

        var result = Guard.AgainstNullOrEmpty(sku, nameof(sku));
        if (result.IsFailure)
            return result;

        result = Guard.AgainstNullOrEmpty(name, nameof(name));
        if (result.IsFailure)
            return result;

        if (unitPrice < 0)
            return Result.Failure(Error.Validation("Validation.Price", $"Price cannot be negative. Parameter name: {nameof(unitPrice)}"));

        var existing = FindItem(productId);
        var newQuantity = (existing?.Quantity ?? 0) + quantity;

        result = ValidateQuantity(quantity);
        if (result.IsFailure)
            return result;

        result = ValidateQuantity(newQuantity);
        if (result.IsFailure)
            return result;

        if (existing is null)
        {
            _items.Add(new CartItem(productId, sku, name, unitPrice, quantity));
        }
        else
        {
            existing.RefreshSnapshot(sku, name, unitPrice);
            existing.SetQuantity(newQuantity);
        }

        return Result.Success();
    }

    public Result ChangeQuantity(Guid productId, int quantity)
    {
        var item = FindItem(productId);
        if (item is null)
            return Result.Failure(ItemNotFound(productId));

        var result = ValidateQuantity(quantity);
        if (result.IsFailure)
            return result;

        item.SetQuantity(quantity);
        return Result.Success();
    }

    public Result RemoveItem(Guid productId)
    {
        var item = FindItem(productId);
        if (item is null)
            return Result.Failure(ItemNotFound(productId));

        _items.Remove(item);
        return Result.Success();
    }

    public void Clear()
    {
        _items.Clear();
    }

    private CartItem? FindItem(Guid productId) => _items.FirstOrDefault(item => item.ProductId == productId);

    private static Result ValidateQuantity(int quantity)
    {
        if (quantity < 1 || quantity > MaxQuantityPerItem)
            return Result.Failure(Error.Validation("Validation.Quantity", $"Quantity must be between 1 and {MaxQuantityPerItem}. Parameter name: quantity"));

        return Result.Success();
    }

    private static Error ItemNotFound(Guid productId) =>
        Error.NotFound("NotFound", $"Product with id {productId} is not in the cart");
}
