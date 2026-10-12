using Bogus;
using Catalog.Domain;

namespace Catalog.UnitTests.Fakers;

public sealed class ProductFaker : Faker<Product>
{
    public ProductFaker()
    {
        CustomInstantiator(f => Product.Create(
            f.Random.Guid(),
            f.Commerce.Ean13(),
            f.Commerce.ProductName(),
            f.Commerce.ProductDescription(),
            f.Random.Decimal(1, 1000)).Value);
    }
}
