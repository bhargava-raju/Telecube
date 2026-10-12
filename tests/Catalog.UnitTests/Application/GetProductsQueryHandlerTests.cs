using Catalog.Application.Abstractions;
using Catalog.Application.Queries.GetProducts;
using Catalog.Domain;
using Catalog.UnitTests.Fakers;
using Moq;

namespace Catalog.UnitTests.Application;

public class GetProductsQueryHandlerTests
{
    private readonly Mock<IProductRepository> _productRepository = new();
    private readonly GetProductsQueryHandler _sut;

    public GetProductsQueryHandlerTests()
    {
        _sut = new GetProductsQueryHandler(_productRepository.Object);
    }

    [Fact]
    public async Task Handle_WhenProductsExist_ReturnsAllProducts()
    {
        // Arrange
        var products = new ProductFaker().Generate(3);
        _productRepository
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);

        // Act
        var result = await _sut.Handle(new GetProductsQuery(), CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(products);
    }

    [Fact]
    public async Task Handle_WhenNoProductsExist_ReturnsEmptyList()
    {
        // Arrange
        _productRepository
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Product>());

        // Act
        var result = await _sut.Handle(new GetProductsQuery(), CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }
}
