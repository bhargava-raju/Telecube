using Telecube.BuildingBlocks.Common;

namespace Catalog.Domain;

public class Category : AggregateRoot
{
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;

    private Category() { }

    // TODO: Validate the inputs and return a new category with a fresh id, or a validation failure.
    public static Result<Category> Create(string name, string description)
    {
        throw new NotImplementedException();
    }

    // TODO: Validate the inputs, change the category's name and description, and return success or a validation failure.
    public Result Update(string name, string description)
    {
        throw new NotImplementedException();
    }
}
