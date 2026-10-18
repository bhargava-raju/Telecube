using Telecube.BuildingBlocks.Common;

namespace Search.Domain;

// Search's read-only copy of a Catalog product. Its Id is the Catalog product id, never a new one.
public class ProductDocument : Entity
{
    public Guid CategoryId { get; private set; }
    public string Sku { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public decimal Price { get; private set; }

    private ProductDocument() { }

    public static Result<ProductDocument> Create(Guid productId, Guid categoryId, string sku, string name, string description, decimal price)
    {
        if (productId == Guid.Empty)
            return Result<ProductDocument>.Failure(Error.Validation("Validation.Required", $"Value cannot be empty. Parameter name: {nameof(productId)}"));

        if (categoryId == Guid.Empty)
            return Result<ProductDocument>.Failure(Error.Validation("Validation.Required", $"Value cannot be empty. Parameter name: {nameof(categoryId)}"));

        var result = Guard.AgainstNullOrEmpty(sku, nameof(sku));
        if (result.IsFailure)
            return Result<ProductDocument>.Failure(result.Error);

        result = Guard.AgainstNullOrEmpty(name, nameof(name));
        if (result.IsFailure)
            return Result<ProductDocument>.Failure(result.Error);

        if (price < 0)
            return Result<ProductDocument>.Failure(Error.Validation("Validation.Price", $"Price cannot be negative. Parameter name: {nameof(price)}"));

        var document = new ProductDocument
        {
            Id = productId,
            CategoryId = categoryId,
            Sku = sku,
            Name = name,
            Description = description ?? string.Empty,
            Price = price
        };

        return Result<ProductDocument>.Success(document);
    }
}
