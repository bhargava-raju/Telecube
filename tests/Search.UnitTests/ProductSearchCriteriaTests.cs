using Search.Domain;
using Telecube.BuildingBlocks.Common;

namespace Search.UnitTests;

public class ProductSearchCriteriaTests
{
    private static readonly Guid ValidCategoryId = Guid.Parse("22222222-2222-2222-2222-222222222222");

    [Fact]
    public void Create_WithValidInput_ReturnsSuccessWithGivenValues()
    {
        // Act
        var result = ProductSearchCriteria.Create("phone", ValidCategoryId, 10m, 500m, 2, 20);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Text.Should().Be("phone");
        result.Value.CategoryId.Should().Be(ValidCategoryId);
        result.Value.MinPrice.Should().Be(10m);
        result.Value.MaxPrice.Should().Be(500m);
        result.Value.Page.Should().Be(2);
        result.Value.PageSize.Should().Be(20);
    }

    [Fact]
    public void Create_WithNoFilters_ReturnsSuccess()
    {
        // Act
        var result = ProductSearchCriteria.Create(null, null, null, null, 1, 10);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Text.Should().BeNull();
        result.Value.CategoryId.Should().BeNull();
        result.Value.MinPrice.Should().BeNull();
        result.Value.MaxPrice.Should().BeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithBlankText_StoresNullText(string text)
    {
        // Act
        var result = ProductSearchCriteria.Create(text, null, null, null, 1, 10);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Text.Should().BeNull();
    }

    [Fact]
    public void Create_WithPaddedText_TrimsText()
    {
        // Act
        var result = ProductSearchCriteria.Create("  phone  ", null, null, null, 1, 10);

        // Assert
        result.Value.Text.Should().Be("phone");
    }

    [Fact]
    public void Create_WithEqualMinAndMaxPrice_ReturnsSuccess()
    {
        // Act
        var result = ProductSearchCriteria.Create(null, null, 100m, 100m, 1, 10);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(ProductSearchCriteria.MaxPageSize)]
    public void Create_WithPageSizeAtLimits_ReturnsSuccess(int pageSize)
    {
        // Act
        var result = ProductSearchCriteria.Create(null, null, null, null, 1, pageSize);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_WithPageBelowOne_ReturnsValidationFailure(int page)
    {
        // Act
        var result = ProductSearchCriteria.Create(null, null, null, null, page, 10);

        // Assert
        AssertValidationFailure(result, "Validation.Page", "page");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(ProductSearchCriteria.MaxPageSize + 1)]
    public void Create_WithPageSizeOutOfRange_ReturnsValidationFailure(int pageSize)
    {
        // Act
        var result = ProductSearchCriteria.Create(null, null, null, null, 1, pageSize);

        // Assert
        AssertValidationFailure(result, "Validation.PageSize", "pageSize");
    }

    [Fact]
    public void Create_WithNegativeMinPrice_ReturnsValidationFailure()
    {
        // Act
        var result = ProductSearchCriteria.Create(null, null, -1m, null, 1, 10);

        // Assert
        AssertValidationFailure(result, "Validation.Price", "minPrice");
    }

    [Fact]
    public void Create_WithNegativeMaxPrice_ReturnsValidationFailure()
    {
        // Act
        var result = ProductSearchCriteria.Create(null, null, null, -1m, 1, 10);

        // Assert
        AssertValidationFailure(result, "Validation.Price", "maxPrice");
    }

    [Fact]
    public void Create_WithMinPriceAboveMaxPrice_ReturnsValidationFailure()
    {
        // Act
        var result = ProductSearchCriteria.Create(null, null, 500m, 10m, 1, 10);

        // Assert
        AssertValidationFailure(result, "Validation.PriceRange", "minPrice");
    }

    private static void AssertValidationFailure(Result<ProductSearchCriteria> result, string expectedCode, string expectedParamName)
    {
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Validation);
        result.Error.Code.Should().Be(expectedCode);
        result.Error.Message.Should().EndWith($"Parameter name: {expectedParamName}");
    }
}
