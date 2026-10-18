using Bogus;
using Search.Domain;

namespace Search.UnitTests.Fakers;

public sealed class ProductDocumentFaker : Faker<ProductDocument>
{
    // TODO: Build each fake document through the document's own creation method using random valid values.
    public ProductDocumentFaker()
    {
    }
}
