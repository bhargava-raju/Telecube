using Moq;
using Search.Application.Abstractions;
using Search.Application.Commands.IndexProduct;
using Search.Domain;
using Telecube.BuildingBlocks.Common;

namespace Search.UnitTests.Application;

public class IndexProductCommandHandlerTests
{
    private readonly Mock<IProductSearchIndex> _searchIndex = new();
    private readonly IndexProductCommandHandler _sut;
    private readonly IndexProductCommand _command = new(
        Guid.NewGuid(), Guid.NewGuid(), "TC-PHONE-001", "Telecube Phone", "A 6.1 inch smartphone", 499.99m);

    public IndexProductCommandHandlerTests()
    {
        _sut = new IndexProductCommandHandler(_searchIndex.Object);
    }

    [Fact]
    public async Task Handle_WhenInputIsValid_UpsertsDocumentAndReturnsSuccess()
    {
        // Act
        var result = await _sut.Handle(_command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _searchIndex.Verify(
            i => i.UpsertAsync(
                It.Is<ProductDocument>(d =>
                    d.Id == _command.ProductId &&
                    d.CategoryId == _command.CategoryId &&
                    d.Sku == _command.Sku &&
                    d.Name == _command.Name &&
                    d.Description == _command.Description &&
                    d.Price == _command.Price),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Theory]
    [InlineData("", "Telecube Phone", 499.99)]
    [InlineData("TC-PHONE-001", "", 499.99)]
    [InlineData("TC-PHONE-001", "Telecube Phone", -1)]
    public async Task Handle_WhenInputIsInvalid_ReturnsValidationFailureAndDoesNotUpsert(
        string sku, string name, decimal price)
    {
        // Arrange
        var command = _command with { Sku = sku, Name = name, Price = price };

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Validation);
        VerifyNotUpserted();
    }

    [Fact]
    public async Task Handle_WhenProductIdIsEmpty_ReturnsValidationFailureAndDoesNotUpsert()
    {
        // Arrange
        var command = _command with { ProductId = Guid.Empty };

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Validation);
        VerifyNotUpserted();
    }

    [Fact]
    public async Task Handle_WhenUpsertThrows_PropagatesException()
    {
        // Arrange
        _searchIndex
            .Setup(i => i.UpsertAsync(It.IsAny<ProductDocument>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Search service unavailable"));

        // Act
        var act = () => _sut.Handle(_command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    private void VerifyNotUpserted()
    {
        _searchIndex.Verify(
            i => i.UpsertAsync(It.IsAny<ProductDocument>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
