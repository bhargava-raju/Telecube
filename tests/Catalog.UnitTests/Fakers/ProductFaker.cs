using Bogus;
using Catalog.Domain;

namespace Catalog.UnitTests.Fakers;

public sealed class ProductFaker : Faker<Product>
{
    // TODO: Build each fake product through the product's own creation method using random valid values.
    public ProductFaker()
    {
    }
}
