namespace Cart.Domain;

// One line in a cart. Sku, Name and UnitPrice are a snapshot of the catalog at the time the item was added.
public class CartItem
{
    public Guid ProductId { get; private set; }
    public string Sku { get; private set; }
    public string Name { get; private set; }
    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }

    public decimal LineTotal => UnitPrice * Quantity;

    // internal: only Cart creates and changes items, so its rules can't be bypassed.
    internal CartItem(Guid productId, string sku, string name, decimal unitPrice, int quantity)
    {
        ProductId = productId;
        Sku = sku;
        Name = name;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }

    internal void RefreshSnapshot(string sku, string name, decimal unitPrice)
    {
        Sku = sku;
        Name = name;
        UnitPrice = unitPrice;
    }

    internal void SetQuantity(int quantity)
    {
        Quantity = quantity;
    }
}
