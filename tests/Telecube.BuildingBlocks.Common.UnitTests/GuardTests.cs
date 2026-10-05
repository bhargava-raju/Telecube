namespace Telecube.BuildingBlocks.Common.UnitTests;

public class GuardTests
{
    [Fact]
    public void AgainstNull_WhenValueIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        object? value = null;

        // Act
        Action act = () => Guard.AgainstNull(value, nameof(value));

        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName(nameof(value));
    }

    [Fact]
    public void AgainstNull_WhenValueIsNotNull_DoesNotThrow()
    {
        // Arrange
        object value = new();

        // Act
        Action act = () => Guard.AgainstNull(value, nameof(value));

        // Assert
        act.Should().NotThrow();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void AgainstNullOrEmpty_WhenValueIsNullOrEmpty_ThrowsArgumentException(string? value)
    {
        // Act
        Action act = () => Guard.AgainstNullOrEmpty(value, nameof(value));

        // Assert
        act.Should().Throw<ArgumentException>().WithParameterName(nameof(value));
    }

    [Fact]
    public void AgainstNullOrEmpty_WhenValueHasText_DoesNotThrow()
    {
        // Arrange
        string value = "Hello, World!";

        // Act
        Action act = () => Guard.AgainstNullOrEmpty(value, nameof(value));

        // Assert
        act.Should().NotThrow();
    }
}
