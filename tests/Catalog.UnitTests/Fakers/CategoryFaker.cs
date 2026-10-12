using Bogus;
using Catalog.Domain;

namespace Catalog.UnitTests.Fakers;

public sealed class CategoryFaker : Faker<Category>
{
    public CategoryFaker()
    {
        CustomInstantiator(f => Category.Create(
            f.Commerce.Department(),
            f.Lorem.Sentence()).Value);
    }
}
