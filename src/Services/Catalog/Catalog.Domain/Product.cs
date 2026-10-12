using Telecube.BuildingBlocks.Common;

namespace Catalog.Domain;

public class Product : AggregateRoot
{
    public Guid CategoryId { get; private set; }
    public string Sku { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public decimal Price { get; private set; }

    private Product() { }

    // TODO: Validate the inputs and return a new product with a fresh id, or a validation failure.
    public static Result<Product> Create(Guid categoryId, string sku, string name, string description, decimal price)
    {
        throw new NotImplementedException();
    }

    // TODO: Validate the inputs, change the product's category, name, description and price, and return success or a validation failure.
    public Result Update(Guid categoryId, string name, string description, decimal price)
    {
        throw new NotImplementedException();
    }
}
