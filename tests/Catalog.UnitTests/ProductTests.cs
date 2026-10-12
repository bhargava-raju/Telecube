using Catalog.Domain;
using Telecube.BuildingBlocks.Common;

namespace Catalog.UnitTests;

public class ProductTests
{
    private static readonly Guid ValidCategoryId = Guid.Parse("22222222-2222-2222-2222-222222222222");
    private static readonly Guid NewCategoryId = Guid.Parse("33333333-3333-3333-3333-333333333333");
    private const string ValidSku = "TC-PHONE-001";
    private const string ValidName = "Telecube Phone";
    private const string ValidDescription = "A 6.1 inch smartphone";
    private const decimal ValidPrice = 499.99m;

    private static Product CreateValidProduct()
    {
        return Product.Create(ValidCategoryId, ValidSku, ValidName, ValidDescription, ValidPrice).Value;
    }

    [Fact]
    public void Create_WithValidInput_ReturnsSuccessWithGivenValues()
    {
        // Act
        var result = Product.Create(ValidCategoryId, ValidSku, ValidName, ValidDescription, ValidPrice);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.CategoryId.Should().Be(ValidCategoryId);
        result.Value.Sku.Should().Be(ValidSku);
        result.Value.Name.Should().Be(ValidName);
        result.Value.Description.Should().Be(ValidDescription);
        result.Value.Price.Should().Be(ValidPrice);
    }

    [Fact]
    public void Create_WithValidInput_AssignsNewId()
    {
        // Act
        var first = CreateValidProduct();
        var second = CreateValidProduct();

        // Assert
        first.Id.Should().NotBeEmpty();
        second.Id.Should().NotBe(first.Id);
    }

    [Fact]
    public void Create_WithZeroPrice_ReturnsSuccess()
    {
        // Act
        var result = Product.Create(ValidCategoryId, ValidSku, ValidName, ValidDescription, 0m);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Theory]
    [InlineData(null, ValidName, ValidDescription, "sku")]
    [InlineData("", ValidName, ValidDescription, "sku")]
    [InlineData(ValidSku, null, ValidDescription, "name")]
    [InlineData(ValidSku, "", ValidDescription, "name")]
    [InlineData(ValidSku, ValidName, null, "description")]
    [InlineData(ValidSku, ValidName, "", "description")]
    public void Create_WhenAnyTextFieldIsNullOrEmpty_ReturnsValidationFailure(
        string? sku, string? name, string? description, string expectedParamName)
    {
        // Act
        var result = Product.Create(ValidCategoryId, sku!, name!, description!, ValidPrice);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Validation);
        result.Error.Message.Should().EndWith($"Parameter name: {expectedParamName}");
    }

    [Fact]
    public void Create_WithEmptyCategoryId_ReturnsValidationFailure()
    {
        // Act
        var result = Product.Create(Guid.Empty, ValidSku, ValidName, ValidDescription, ValidPrice);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Validation);
        result.Error.Message.Should().EndWith("Parameter name: categoryId");
    }

    [Fact]
    public void Create_WithNegativePrice_ReturnsValidationFailure()
    {
        // Act
        var result = Product.Create(ValidCategoryId, ValidSku, ValidName, ValidDescription, -1m);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Validation);
        result.Error.Message.Should().EndWith("Parameter name: price");
    }

    [Fact]
    public void Update_WithValidInput_ChangesCategoryNameDescriptionAndPrice()
    {
        // Arrange
        var product = CreateValidProduct();

        // Act
        var result = product.Update(NewCategoryId, "New Name", "New description", 10m);

        // Assert
        result.IsSuccess.Should().BeTrue();
        product.CategoryId.Should().Be(NewCategoryId);
        product.Name.Should().Be("New Name");
        product.Description.Should().Be("New description");
        product.Price.Should().Be(10m);
    }

    [Fact]
    public void Update_WithValidInput_KeepsSkuAndId()
    {
        // Arrange
        var product = CreateValidProduct();
        var originalId = product.Id;

        // Act
        product.Update(NewCategoryId, "New Name", "New description", 10m);

        // Assert
        product.Id.Should().Be(originalId);
        product.Sku.Should().Be(ValidSku);
    }

    [Theory]
    [InlineData("", ValidDescription, 1, "name")]
    [InlineData(ValidName, "", 1, "description")]
    [InlineData(ValidName, ValidDescription, -1, "price")]
    public void Update_WhenInputIsInvalid_ReturnsFailureAndLeavesProductUnchanged(
        string name, string description, int price, string expectedParamName)
    {
        // Arrange
        var product = CreateValidProduct();

        // Act
        var result = product.Update(NewCategoryId, name, description, price);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Message.Should().EndWith($"Parameter name: {expectedParamName}");
        product.CategoryId.Should().Be(ValidCategoryId);
        product.Name.Should().Be(ValidName);
        product.Description.Should().Be(ValidDescription);
        product.Price.Should().Be(ValidPrice);
    }

    [Fact]
    public void Update_WithEmptyCategoryId_ReturnsValidationFailure()
    {
        // Arrange
        var product = CreateValidProduct();

        // Act
        var result = product.Update(Guid.Empty, ValidName, ValidDescription, ValidPrice);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Message.Should().EndWith("Parameter name: categoryId");
        product.CategoryId.Should().Be(ValidCategoryId);
    }
}
