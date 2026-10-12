using Catalog.Application.Abstractions;
using Catalog.Application.Commands.DeleteProduct;
using Catalog.UnitTests.Fakers;
using Moq;
using Telecube.BuildingBlocks.Common;
using Telecube.Contracts;

namespace Catalog.UnitTests.Application;

public class DeleteProductCommandHandlerTests
{
    private readonly Mock<IProductRepository> _productRepository = new();
    private readonly Mock<IIntegrationEventPublisher> _publisher = new();
    private readonly DeleteProductCommandHandler _sut;

    public DeleteProductCommandHandlerTests()
    {
        _sut = new DeleteProductCommandHandler(_productRepository.Object, _publisher.Object);
    }

    [Fact]
    public async Task Handle_WhenProductExists_DeletesAndPublishesProductDeleted()
    {
        // Arrange
        var product = new ProductFaker().Generate();
        _productRepository
            .Setup(r => r.GetByIdAsync(product.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        // Act
        var result = await _sut.Handle(new DeleteProductCommand(product.Id), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _productRepository.Verify(
            r => r.DeleteAsync(product.Id, It.IsAny<CancellationToken>()),
            Times.Once);
        _publisher.Verify(
            p => p.PublishAsync(
                It.Is<ProductDeleted>(e => e.ProductId == product.Id),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenProductDoesNotExist_ReturnsNotFoundAndDoesNotPublish()
    {
        // Act
        var result = await _sut.Handle(new DeleteProductCommand(Guid.NewGuid()), CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.NotFound);

        _productRepository.Verify(
            r => r.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Never);
        _publisher.Verify(
            p => p.PublishAsync(It.IsAny<ProductDeleted>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
