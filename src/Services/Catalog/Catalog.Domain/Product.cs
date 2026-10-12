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

    public static Result<Product> Create(Guid categoryId, string sku, string name, string description, decimal price)
    {
        var result = Guard.AgainstNullOrEmpty(sku, nameof(sku));
        if (result.IsFailure)
            return Result<Product>.Failure(result.Error);

        result = Validate(categoryId, name, description, price);
        if (result.IsFailure)
            return Result<Product>.Failure(result.Error);

        var product = new Product
        {
            Id = Guid.NewGuid(),
            CategoryId = categoryId,
            Sku = sku,
            Name = name,
            Description = description,
            Price = price
        };

        return Result<Product>.Success(product);
    }

    public Result Update(Guid categoryId, string name, string description, decimal price)
    {
        var result = Validate(categoryId, name, description, price);
        if (result.IsFailure)
            return result;

        CategoryId = categoryId;
        Name = name;
        Description = description;
        Price = price;

        return Result.Success();
    }

    // Rules shared by Create and Update. A Guid is never null, so an unset category id shows up as Guid.Empty.
    private static Result Validate(Guid categoryId, string name, string description, decimal price)
    {
        if (categoryId == Guid.Empty)
            return Result.Failure(Error.Validation("Validation.Required", $"Value cannot be empty. Parameter name: {nameof(categoryId)}"));

        var result = Guard.AgainstNullOrEmpty(name, nameof(name));
        if (result.IsFailure)
            return result;

        result = Guard.AgainstNullOrEmpty(description, nameof(description));
        if (result.IsFailure)
            return result;

        if (price < 0)
            return Result.Failure(Error.Validation("Validation.Price", $"Price cannot be negative. Parameter name: {nameof(price)}"));

        return Result.Success();
    }
}
