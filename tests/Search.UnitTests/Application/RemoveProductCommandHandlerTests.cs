using Moq;
using Search.Application.Abstractions;
using Search.Application.Commands.RemoveProduct;

namespace Search.UnitTests.Application;

public class RemoveProductCommandHandlerTests
{
    private readonly Mock<IProductSearchIndex> _searchIndex = new();
    private readonly RemoveProductCommandHandler _sut;

    public RemoveProductCommandHandlerTests()
    {
        _sut = new RemoveProductCommandHandler(_searchIndex.Object);
    }

    [Fact]
    public async Task Handle_DeletesDocumentAndReturnsSuccess()
    {
        // Arrange
        var command = new RemoveProductCommand(Guid.NewGuid());

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _searchIndex.Verify(
            i => i.DeleteAsync(command.ProductId, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenDeleteThrows_PropagatesException()
    {
        // Arrange
        _searchIndex
            .Setup(i => i.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Search service unavailable"));

        // Act
        var act = () => _sut.Handle(new RemoveProductCommand(Guid.NewGuid()), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}
