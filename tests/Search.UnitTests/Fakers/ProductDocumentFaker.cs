using Bogus;
using Search.Domain;

namespace Search.UnitTests.Fakers;

public sealed class ProductDocumentFaker : Faker<ProductDocument>
{
    public ProductDocumentFaker()
    {
        CustomInstantiator(f => ProductDocument.Create(
            f.Random.Guid(),
            f.Random.Guid(),
            f.Commerce.Ean13(),
            f.Commerce.ProductName(),
            f.Commerce.ProductDescription(),
            f.Random.Decimal(1, 1000)).Value);
    }
}
