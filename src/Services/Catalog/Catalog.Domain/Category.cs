using Telecube.BuildingBlocks.Common;

namespace Catalog.Domain;

public class Category : AggregateRoot
{
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;

    private Category() { }

    public static Result<Category> Create(string name, string description)
    {
        var validationResult = Guard.AgainstNullOrEmpty(name, nameof(name));
        if (!validationResult.IsSuccess)
        {
            return Result<Category>.Failure(validationResult.Error);
        }

        validationResult = Guard.AgainstNullOrEmpty(description, nameof(description));
        if (!validationResult.IsSuccess)
        {
            return Result<Category>.Failure(validationResult.Error);
        }

        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description
        };

        return Result<Category>.Success(category);
    }

    public Result Update(string name, string description)
    {
        var validationResult = Guard.AgainstNullOrEmpty(name, nameof(name));
        if (!validationResult.IsSuccess)
        {
            return Result.Failure(validationResult.Error);
        }

        validationResult = Guard.AgainstNullOrEmpty(description, nameof(description));
        if (!validationResult.IsSuccess)
        {
            return Result.Failure(validationResult.Error);
        }

        Name = name;
        Description = description;

        return Result.Success();
    }
}
