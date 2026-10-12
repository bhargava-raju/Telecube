using Catalog.Application.Abstractions;
using Catalog.Application.Commands.CreateCategory;
using Catalog.Domain;
using Catalog.UnitTests.Fakers;
using Moq;
using Telecube.BuildingBlocks.Common;

namespace Catalog.UnitTests.Application;

public class CreateCategoryCommandHandlerTests
{
    private readonly Mock<ICategoryRepository> _categoryRepository = new();
    private readonly CreateCategoryCommandHandler _sut;
    private readonly CreateCategoryCommand _command = new("Smartphones", "Phones and accessories");

    public CreateCategoryCommandHandlerTests()
    {
        _sut = new CreateCategoryCommandHandler(_categoryRepository.Object);
    }

    [Fact]
    public async Task Handle_WhenNameIsUnique_AddsCategoryAndReturnsSuccess()
    {
        // Act
        var result = await _sut.Handle(_command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Should().Be(_command.Name);
        result.Value.Description.Should().Be(_command.Description);

        _categoryRepository.Verify(
            r => r.AddAsync(result.Value, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenNameAlreadyExists_ReturnsConflictAndDoesNotAdd()
    {
        // Arrange
        _categoryRepository
            .Setup(r => r.GetByNameAsync(_command.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CategoryFaker().Generate());

        // Act
        var result = await _sut.Handle(_command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Conflict);

        _categoryRepository.Verify(
            r => r.AddAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Theory]
    [InlineData("", "Phones and accessories")]
    [InlineData("Smartphones", "")]
    public async Task Handle_WhenInputIsInvalid_ReturnsValidationFailureAndDoesNotAdd(string name, string description)
    {
        // Arrange
        var command = new CreateCategoryCommand(name, description);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Validation);

        _categoryRepository.Verify(
            r => r.AddAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
