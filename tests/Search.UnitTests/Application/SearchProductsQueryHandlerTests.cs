using Moq;
using Search.Application.Abstractions;
using Search.Application.Queries.SearchProducts;
using Search.Domain;
using Search.UnitTests.Fakers;
using Telecube.BuildingBlocks.Common;

namespace Search.UnitTests.Application;

public class SearchProductsQueryHandlerTests
{
    private readonly Mock<IProductSearchIndex> _searchIndex = new();
    private readonly SearchProductsQueryHandler _sut;
    private readonly SearchProductsQuery _query = new("  phone  ", Guid.NewGuid(), 10m, 500m, 2, 20);

    public SearchProductsQueryHandlerTests()
    {
        _sut = new SearchProductsQueryHandler(_searchIndex.Object);
    }

    [Fact]
    public async Task Handle_WhenCriteriaAreValid_ReturnsIndexResult()
    {
        // Arrange
        var expected = new ProductSearchResult(new ProductDocumentFaker().Generate(3), 42);
        _searchIndex
            .Setup(i => i.SearchAsync(It.IsAny<ProductSearchCriteria>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // Act
        var result = await _sut.Handle(_query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeSameAs(expected);
    }

    [Fact]
    public async Task Handle_WhenCriteriaAreValid_PassesCriteriaToIndex()
    {
        // Arrange
        _searchIndex
            .Setup(i => i.SearchAsync(It.IsAny<ProductSearchCriteria>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ProductSearchResult([], 0));

        // Act
        await _sut.Handle(_query, CancellationToken.None);

        // Assert
        _searchIndex.Verify(
            i => i.SearchAsync(
                It.Is<ProductSearchCriteria>(c =>
                    c.Text == "phone" &&
                    c.CategoryId == _query.CategoryId &&
                    c.MinPrice == _query.MinPrice &&
                    c.MaxPrice == _query.MaxPrice &&
                    c.Page == _query.Page &&
                    c.PageSize == _query.PageSize),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Theory]
    [InlineData(0, 20, null, null)]
    [InlineData(1, 0, null, null)]
    [InlineData(1, 20, -1.0, null)]
    [InlineData(1, 20, 500.0, 10.0)]
    public async Task Handle_WhenCriteriaAreInvalid_ReturnsValidationFailureAndDoesNotQueryIndex(
        int page, int pageSize, double? minPrice, double? maxPrice)
    {
        // Arrange
        var query = _query with
        {
            Page = page,
            PageSize = pageSize,
            MinPrice = (decimal?)minPrice,
            MaxPrice = (decimal?)maxPrice
        };

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Failure);
        _searchIndex.Verify(
            i => i.SearchAsync(It.IsAny<ProductSearchCriteria>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
