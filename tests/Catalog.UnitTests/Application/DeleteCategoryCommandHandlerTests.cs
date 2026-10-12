using Catalog.Application.Abstractions;
using Catalog.Application.Commands.DeleteCategory;
using Catalog.UnitTests.Fakers;
using Moq;
using Telecube.BuildingBlocks.Common;

namespace Catalog.UnitTests.Application;

public class DeleteCategoryCommandHandlerTests
{
    private readonly Mock<ICategoryRepository> _categoryRepository = new();
    private readonly DeleteCategoryCommandHandler _sut;

    public DeleteCategoryCommandHandlerTests()
    {
        _sut = new DeleteCategoryCommandHandler(_categoryRepository.Object);
    }

    [Fact]
    public async Task Handle_WhenCategoryExistsWithoutProducts_DeletesCategory()
    {
        // Arrange
        var category = new CategoryFaker().Generate();
        _categoryRepository
            .Setup(r => r.GetByIdAsync(category.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        // Act
        var result = await _sut.Handle(new DeleteCategoryCommand(category.Id), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _categoryRepository.Verify(
            r => r.DeleteAsync(category.Id, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenCategoryDoesNotExist_ReturnsNotFoundAndDoesNotDelete()
    {
        // Act
        var result = await _sut.Handle(new DeleteCategoryCommand(Guid.NewGuid()), CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.NotFound);

        _categoryRepository.Verify(
            r => r.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenCategoryStillHasProducts_ReturnsConflictAndDoesNotDelete()
    {
        // Arrange
        var category = new CategoryFaker().Generate();
        _categoryRepository
            .Setup(r => r.GetByIdAsync(category.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);
        _categoryRepository
            .Setup(r => r.HasProductsAsync(category.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _sut.Handle(new DeleteCategoryCommand(category.Id), CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Conflict);

        _categoryRepository.Verify(
            r => r.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
