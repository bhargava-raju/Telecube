using Catalog.Domain;
using Telecube.BuildingBlocks.Common;

namespace Catalog.UnitTests;

public class CategoryTests
{
    private const string ValidName = "Phones";
    private const string ValidDescription = "Smartphones and feature phones";

    private static Category CreateValidCategory()
    {
        return Category.Create(ValidName, ValidDescription).Value;
    }

    [Fact]
    public void Create_WithValidInput_ReturnsSuccessWithGivenValues()
    {
        // Act
        var result = Category.Create(ValidName, ValidDescription);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Should().Be(ValidName);
        result.Value.Description.Should().Be(ValidDescription);
        result.Value.Id.Should().NotBeEmpty();
    }

    [Theory]
    [InlineData(null, ValidDescription, "name")]
    [InlineData("", ValidDescription, "name")]
    [InlineData(ValidName, null, "description")]
    [InlineData(ValidName, "", "description")]
    public void Create_WhenAnyFieldIsNullOrEmpty_ReturnsValidationFailure(
        string? name, string? description, string expectedParamName)
    {
        // Act
        var result = Category.Create(name!, description!);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Validation);
        result.Error.Message.Should().EndWith($"Parameter name: {expectedParamName}");
    }

    [Fact]
    public void Update_WithValidInput_ChangesNameAndDescription()
    {
        // Arrange
        var category = CreateValidCategory();

        // Act
        var result = category.Update("Tablets", "Tablets and e-readers");

        // Assert
        result.IsSuccess.Should().BeTrue();
        category.Name.Should().Be("Tablets");
        category.Description.Should().Be("Tablets and e-readers");
    }

    [Theory]
    [InlineData("", ValidDescription, "name")]
    [InlineData(ValidName, "", "description")]
    public void Update_WhenInputIsInvalid_ReturnsFailureAndLeavesCategoryUnchanged(
        string name, string description, string expectedParamName)
    {
        // Arrange
        var category = CreateValidCategory();

        // Act
        var result = category.Update(name, description);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Message.Should().EndWith($"Parameter name: {expectedParamName}");
        category.Name.Should().Be(ValidName);
        category.Description.Should().Be(ValidDescription);
    }
}
