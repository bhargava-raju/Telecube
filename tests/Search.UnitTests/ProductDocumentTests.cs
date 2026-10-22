using Search.Domain;
using Telecube.BuildingBlocks.Common;

namespace Search.UnitTests;

public class ProductDocumentTests
{
    private static readonly Guid ValidProductId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    private static readonly Guid ValidCategoryId = Guid.Parse("22222222-2222-2222-2222-222222222222");
    private const string ValidSku = "TC-PHONE-001";
    private const string ValidName = "Telecube Phone";
    private const string ValidDescription = "A 6.1 inch smartphone";
    private const decimal ValidPrice = 499.99m;

    [Fact]
    public void Create_WithValidInput_ReturnsSuccessWithGivenValues()
    {
        // Act
        var result = ProductDocument.Create(ValidProductId, ValidCategoryId, ValidSku, ValidName, ValidDescription, ValidPrice);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.CategoryId.Should().Be(ValidCategoryId);
        result.Value.Sku.Should().Be(ValidSku);
        result.Value.Name.Should().Be(ValidName);
        result.Value.Description.Should().Be(ValidDescription);
        result.Value.Price.Should().Be(ValidPrice);
    }

    [Fact]
    public void Create_WithValidInput_UsesCatalogProductIdAsId()
    {
        // Act
        var result = ProductDocument.Create(ValidProductId, ValidCategoryId, ValidSku, ValidName, ValidDescription, ValidPrice);

        // Assert
        result.Value.Id.Should().Be(ValidProductId);
    }

    [Fact]
    public void Create_WithZeroPrice_ReturnsSuccess()
    {
        // Act
        var result = ProductDocument.Create(ValidProductId, ValidCategoryId, ValidSku, ValidName, ValidDescription, 0m);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Price.Should().Be(0m);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Create_WithMissingDescription_StoresEmptyDescription(string? description)
    {
        // Act
        var result = ProductDocument.Create(ValidProductId, ValidCategoryId, ValidSku, ValidName, description!, ValidPrice);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Description.Should().BeEmpty();
    }

    [Fact]
    public void Create_WithEmptyProductId_ReturnsValidationFailure()
    {
        // Act
        var result = ProductDocument.Create(Guid.Empty, ValidCategoryId, ValidSku, ValidName, ValidDescription, ValidPrice);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Validation);
        result.Error.Message.Should().EndWith("Parameter name: productId");
    }

    [Fact]
    public void Create_WithEmptyCategoryId_ReturnsValidationFailure()
    {
        // Act
        var result = ProductDocument.Create(ValidProductId, Guid.Empty, ValidSku, ValidName, ValidDescription, ValidPrice);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Validation);
        result.Error.Message.Should().EndWith("Parameter name: categoryId");
    }

    [Theory]
    [InlineData(null, ValidName, "sku")]
    [InlineData("", ValidName, "sku")]
    [InlineData(ValidSku, null, "name")]
    [InlineData(ValidSku, "", "name")]
    public void Create_WithMissingText_ReturnsValidationFailure(string? sku, string? name, string expectedParamName)
    {
        // Act
        var result = ProductDocument.Create(ValidProductId, ValidCategoryId, sku!, name!, ValidDescription, ValidPrice);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Validation);
        result.Error.Message.Should().EndWith($"Parameter name: {expectedParamName}");
    }

    [Fact]
    public void Create_WithNegativePrice_ReturnsValidationFailure()
    {
        // Act
        var result = ProductDocument.Create(ValidProductId, ValidCategoryId, ValidSku, ValidName, ValidDescription, -0.01m);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Validation);
        result.Error.Code.Should().Be("Validation.Price");
        result.Error.Message.Should().EndWith("Parameter name: price");
    }
}
