using Catalog.Application.Abstractions;
using Catalog.Application.Queries.GetCategoryById;
using Catalog.UnitTests.Fakers;
using Moq;
using Telecube.BuildingBlocks.Common;

namespace Catalog.UnitTests.Application;

public class GetCategoryByIdQueryHandlerTests
{
    private readonly Mock<ICategoryRepository> _categoryRepository = new();
    private readonly GetCategoryByIdQueryHandler _sut;

    public GetCategoryByIdQueryHandlerTests()
    {
        _sut = new GetCategoryByIdQueryHandler(_categoryRepository.Object);
    }

    [Fact]
    public async Task Handle_WhenCategoryExists_ReturnsCategory()
    {
        // Arrange
        var category = new CategoryFaker().Generate();
        _categoryRepository
            .Setup(r => r.GetByIdAsync(category.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        // Act
        var result = await _sut.Handle(new GetCategoryByIdQuery(category.Id), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeSameAs(category);
    }

    [Fact]
    public async Task Handle_WhenCategoryDoesNotExist_ReturnsNotFound()
    {
        // Act
        var result = await _sut.Handle(new GetCategoryByIdQuery(Guid.NewGuid()), CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.NotFound);
    }
}
