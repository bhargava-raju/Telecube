using Catalog.Application.Abstractions;
using Catalog.Application.Commands.UpdateProduct;
using Catalog.Domain;
using Catalog.UnitTests.Fakers;
using Moq;
using Telecube.BuildingBlocks.Common;
using Telecube.Contracts;

namespace Catalog.UnitTests.Application;

public class UpdateProductCommandHandlerTests
{
    private const string NewName = "Telecube Phone Pro";
    private const string NewDescription = "A 6.7 inch smartphone";
    private const decimal NewPrice = 799.99m;

    private readonly Mock<IProductRepository> _productRepository = new();
    private readonly Mock<ICategoryRepository> _categoryRepository = new();
    private readonly Mock<IIntegrationEventPublisher> _publisher = new();
    private readonly UpdateProductCommandHandler _sut;
    private readonly Product _product = new ProductFaker().Generate();
    private readonly Category _category = new CategoryFaker().Generate();
    private readonly UpdateProductCommand _command;

    public UpdateProductCommandHandlerTests()
    {
        _sut = new UpdateProductCommandHandler(_productRepository.Object, _categoryRepository.Object, _publisher.Object);
        _command = new UpdateProductCommand(_product.Id, _category.Id, NewName, NewDescription, NewPrice);

        _productRepository
            .Setup(r => r.GetByIdAsync(_product.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_product);
        _categoryRepository
            .Setup(r => r.GetByIdAsync(_category.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_category);
    }

    [Fact]
    public async Task Handle_WhenProductAndCategoryExist_UpdatesAndPublishesProductUpdated()
    {
        // Act
        var result = await _sut.Handle(_command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _product.CategoryId.Should().Be(_category.Id);
        _product.Name.Should().Be(NewName);
        _product.Description.Should().Be(NewDescription);
        _product.Price.Should().Be(NewPrice);

        _productRepository.Verify(
            r => r.UpdateAsync(_product, It.IsAny<CancellationToken>()),
            Times.Once);
        _publisher.Verify(
            p => p.PublishAsync(
                It.Is<ProductUpdated>(e =>
                    e.ProductId == _product.Id &&
                    e.CategoryId == _category.Id &&
                    e.Sku == _product.Sku &&
                    e.Name == NewName &&
                    e.Description == NewDescription &&
                    e.Price == NewPrice),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenProductDoesNotExist_ReturnsNotFoundAndDoesNotPublish()
    {
        // Arrange
        var command = _command with { Id = Guid.NewGuid() };

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.NotFound);
        VerifyNothingSavedOrPublished();
    }

    [Fact]
    public async Task Handle_WhenCategoryDoesNotExist_ReturnsNotFoundAndDoesNotPublish()
    {
        // Arrange
        var command = _command with { CategoryId = Guid.NewGuid() };

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.NotFound);
        VerifyNothingSavedOrPublished();
    }

    [Fact]
    public async Task Handle_WhenInputIsInvalid_ReturnsValidationFailureAndDoesNotPublish()
    {
        // Arrange
        var command = _command with { Price = -1 };

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Validation);
        VerifyNothingSavedOrPublished();
    }

    [Fact]
    public async Task Handle_WhenSaveThrows_PropagatesExceptionAndDoesNotPublish()
    {
        // Arrange
        _productRepository
            .Setup(r => r.UpdateAsync(_product, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Database unavailable"));

        // Act
        var act = () => _sut.Handle(_command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
        _publisher.Verify(
            p => p.PublishAsync(It.IsAny<ProductUpdated>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    private void VerifyNothingSavedOrPublished()
    {
        _productRepository.Verify(
            r => r.UpdateAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()),
            Times.Never);
        _publisher.Verify(
            p => p.PublishAsync(It.IsAny<ProductUpdated>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
