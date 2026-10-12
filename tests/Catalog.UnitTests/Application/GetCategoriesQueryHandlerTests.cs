using Catalog.Application.Abstractions;
using Catalog.Application.Queries.GetCategories;
using Catalog.Domain;
using Catalog.UnitTests.Fakers;
using Moq;

namespace Catalog.UnitTests.Application;

public class GetCategoriesQueryHandlerTests
{
    private readonly Mock<ICategoryRepository> _categoryRepository = new();
    private readonly GetCategoriesQueryHandler _sut;

    public GetCategoriesQueryHandlerTests()
    {
        _sut = new GetCategoriesQueryHandler(_categoryRepository.Object);
    }

    [Fact]
    public async Task Handle_WhenCategoriesExist_ReturnsAllCategories()
    {
        // Arrange
        var categories = new CategoryFaker().Generate(3);
        _categoryRepository
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(categories);

        // Act
        var result = await _sut.Handle(new GetCategoriesQuery(), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(categories);
    }

    [Fact]
    public async Task Handle_WhenNoCategoriesExist_ReturnsEmptyList()
    {
        // Arrange
        _categoryRepository
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Category>());

        // Act
        var result = await _sut.Handle(new GetCategoriesQuery(), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }
}
