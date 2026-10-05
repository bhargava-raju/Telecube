namespace Telecube.BuildingBlocks.Common.UnitTests;

public class GuardTests
{
    [Fact]
    public void AgainstNull_WhenValueIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        object? value = null;

        // Act
        var result = Guard.AgainstNull(value, nameof(value));

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Validation);
        result.Error.Message.Should().Be($"Value cannot be null. Parameter name: {nameof(value)}");

    }

    [Fact]
    public void AgainstNull_WhenValueIsNotNull_DoesNotThrow()
    {
        // Arrange
        object value = new();

        // Act
        var result = Guard.AgainstNull(value, nameof(value));

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Error.Should().Be(Error.None);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void AgainstNullOrEmpty_WhenValueIsNullOrEmpty_ThrowsArgumentException(string? value)
    {
        // Act
        var result = Guard.AgainstNullOrEmpty(value, nameof(value));

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Validation);
        result.Error.Message.Should().Be($"Value cannot be empty. Parameter name: {nameof(value)}");
    }

    [Fact]
    public void AgainstNullOrEmpty_WhenValueHasText_DoesNotThrow()
    {
        // Arrange
        string value = "Hello, World!";

        // Act
        var result = Guard.AgainstNullOrEmpty(value, nameof(value));

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Error.Should().Be(Error.None);
    }
}
