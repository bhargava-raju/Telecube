using Catalog.Application.Abstractions;
using Catalog.Application.Commands.CreateProduct;
using Catalog.Domain;
using Catalog.UnitTests.Fakers;
using Moq;
using Telecube.BuildingBlocks.Common;
using Telecube.Contracts;

namespace Catalog.UnitTests.Application;

public class CreateProductCommandHandlerTests
{
    private readonly Mock<IProductRepository> _productRepository = new();
    private readonly Mock<ICategoryRepository> _categoryRepository = new();
    private readonly Mock<IIntegrationEventPublisher> _publisher = new();
    private readonly CreateProductCommandHandler _sut;
    private readonly Category _category = new CategoryFaker().Generate();
    private readonly CreateProductCommand _command;

    public CreateProductCommandHandlerTests()
    {
        _sut = new CreateProductCommandHandler(_productRepository.Object, _categoryRepository.Object, _publisher.Object);
        _command = new CreateProductCommand(_category.Id, "TC-PHONE-001", "Telecube Phone", "A 6.1 inch smartphone", 499.99m);

        _categoryRepository
            .Setup(r => r.GetByIdAsync(_category.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_category);
    }

    [Fact]
    public async Task Handle_WhenInputIsValid_AddsProductAndPublishesProductCreated()
    {
        // Act
        var result = await _sut.Handle(_command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.CategoryId.Should().Be(_command.CategoryId);
        result.Value.Sku.Should().Be(_command.Sku);
        result.Value.Name.Should().Be(_command.Name);
        result.Value.Description.Should().Be(_command.Description);
        result.Value.Price.Should().Be(_command.Price);

        _productRepository.Verify(
            r => r.AddAsync(result.Value, It.IsAny<CancellationToken>()),
            Times.Once);
        _publisher.Verify(
            p => p.PublishAsync(
                It.Is<ProductCreated>(e =>
                    e.ProductId == result.Value.Id &&
                    e.CategoryId == _command.CategoryId &&
                    e.Sku == _command.Sku &&
                    e.Name == _command.Name &&
                    e.Description == _command.Description &&
                    e.Price == _command.Price),
                It.IsAny<CancellationToken>()),
            Times.Once);
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
    public async Task Handle_WhenSkuAlreadyExists_ReturnsConflictAndDoesNotPublish()
    {
        // Arrange
        _productRepository
            .Setup(r => r.GetBySkuAsync(_command.Sku, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ProductFaker().Generate());

        // Act
        var result = await _sut.Handle(_command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Conflict);
        VerifyNothingSavedOrPublished();
    }

    [Theory]
    [InlineData("", "Telecube Phone", "A 6.1 inch smartphone", 499.99)]
    [InlineData("TC-PHONE-001", "", "A 6.1 inch smartphone", 499.99)]
    [InlineData("TC-PHONE-001", "Telecube Phone", "", 499.99)]
    [InlineData("TC-PHONE-001", "Telecube Phone", "A 6.1 inch smartphone", -1)]
    public async Task Handle_WhenInputIsInvalid_ReturnsValidationFailureAndDoesNotPublish(
        string sku, string name, string description, decimal price)
    {
        // Arrange
        var command = new CreateProductCommand(_category.Id, sku, name, description, price);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Validation);
        VerifyNothingSavedOrPublished();
    }

    [Fact]
    public async Task Handle_WhenAddThrows_PropagatesExceptionAndDoesNotPublish()
    {
        // Arrange
        _productRepository
            .Setup(r => r.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Database unavailable"));

        // Act
        var act = () => _sut.Handle(_command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
        _publisher.Verify(
            p => p.PublishAsync(It.IsAny<ProductCreated>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    private void VerifyNothingSavedOrPublished()
    {
        _productRepository.Verify(
            r => r.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()),
            Times.Never);
        _publisher.Verify(
            p => p.PublishAsync(It.IsAny<ProductCreated>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
