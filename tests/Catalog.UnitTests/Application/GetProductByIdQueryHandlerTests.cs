using Catalog.Application.Abstractions;
using Catalog.Application.Queries.GetProductById;
using Catalog.UnitTests.Fakers;
using Moq;
using Telecube.BuildingBlocks.Common;

namespace Catalog.UnitTests.Application;

public class GetProductByIdQueryHandlerTests
{
    private readonly Mock<IProductRepository> _productRepository = new();
    private readonly GetProductByIdQueryHandler _sut;

    public GetProductByIdQueryHandlerTests()
    {
        _sut = new GetProductByIdQueryHandler(_productRepository.Object);
    }

    [Fact]
    public async Task Handle_WhenProductExists_ReturnsProduct()
    {
        // Arrange
        var product = new ProductFaker().Generate();
        _productRepository
            .Setup(r => r.GetByIdAsync(product.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        // Act
        var result = await _sut.Handle(new GetProductByIdQuery(product.Id), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeSameAs(product);
    }

    [Fact]
    public async Task Handle_WhenProductDoesNotExist_ReturnsNotFound()
    {
        // Act
        var result = await _sut.Handle(new GetProductByIdQuery(Guid.NewGuid()), CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.NotFound);
    }
}
