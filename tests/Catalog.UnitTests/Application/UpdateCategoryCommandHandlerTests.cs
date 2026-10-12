using Catalog.Application.Abstractions;
using Catalog.Application.Commands.UpdateCategory;
using Catalog.Domain;
using Catalog.UnitTests.Fakers;
using Moq;
using Telecube.BuildingBlocks.Common;

namespace Catalog.UnitTests.Application;

public class UpdateCategoryCommandHandlerTests
{
    private const string NewName = "Tablets";
    private const string NewDescription = "Tablets and e-readers";

    private readonly Mock<ICategoryRepository> _categoryRepository = new();
    private readonly UpdateCategoryCommandHandler _sut;

    public UpdateCategoryCommandHandlerTests()
    {
        _sut = new UpdateCategoryCommandHandler(_categoryRepository.Object);
    }

    [Fact]
    public async Task Handle_WhenCategoryExists_UpdatesCategory()
    {
        // Arrange
        var category = new CategoryFaker().Generate();
        _categoryRepository
            .Setup(r => r.GetByIdAsync(category.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);
        var command = new UpdateCategoryCommand(category.Id, NewName, NewDescription);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        category.Name.Should().Be(NewName);
        category.Description.Should().Be(NewDescription);

        _categoryRepository.Verify(
            r => r.UpdateAsync(category, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenNameBelongsToSameCategory_UpdatesCategory()
    {
        // Arrange
        var category = new CategoryFaker().Generate();
        _categoryRepository
            .Setup(r => r.GetByIdAsync(category.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);
        _categoryRepository
            .Setup(r => r.GetByNameAsync(category.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);
        var command = new UpdateCategoryCommand(category.Id, category.Name, NewDescription);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _categoryRepository.Verify(
            r => r.UpdateAsync(category, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenCategoryDoesNotExist_ReturnsNotFoundAndDoesNotUpdate()
    {
        // Arrange
        var command = new UpdateCategoryCommand(Guid.NewGuid(), NewName, NewDescription);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.NotFound);

        _categoryRepository.Verify(
            r => r.UpdateAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenNameBelongsToAnotherCategory_ReturnsConflictAndDoesNotUpdate()
    {
        // Arrange
        var category = new CategoryFaker().Generate();
        _categoryRepository
            .Setup(r => r.GetByIdAsync(category.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);
        _categoryRepository
            .Setup(r => r.GetByNameAsync(NewName, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CategoryFaker().Generate());
        var command = new UpdateCategoryCommand(category.Id, NewName, NewDescription);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Conflict);

        _categoryRepository.Verify(
            r => r.UpdateAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenInputIsInvalid_ReturnsValidationFailureAndDoesNotUpdate()
    {
        // Arrange
        var category = new CategoryFaker().Generate();
        _categoryRepository
            .Setup(r => r.GetByIdAsync(category.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);
        var command = new UpdateCategoryCommand(category.Id, NewName, "");

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Validation);

        _categoryRepository.Verify(
            r => r.UpdateAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
