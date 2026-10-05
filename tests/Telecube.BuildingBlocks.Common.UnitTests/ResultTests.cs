namespace Telecube.BuildingBlocks.Common.UnitTests;

public class ResultTests
{
    private static readonly Error SomeError = new("code", "Some error", ErrorType.Conflict);

    [Fact]
    public void Success_ReturnsSuccessfulResultWithNoError()
    {
        // Act
        var result = Result.Success();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Error.Should().Be(Error.None);
    }

    [Fact]
    public void Failure_ReturnsFailedResultWithGivenError()
    {
        // Act
        var result = Result.Failure(SomeError);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(SomeError);
    }

    [Fact]
    public void GenericSuccess_ExposesValue()
    {
        // Act
        var result = Result<int>.Success(42);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);
    }

    [Fact]
    public void GenericFailure_AccessingValue_ThrowsInvalidOperationException()
    {
        // Arrange
        var result = Result<int>.Failure(SomeError);

        // Act
        Func<int> act = () => result.Value;

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Match_WhenSuccess_CallsOnSuccess()
    {
        // Arrange
        var result = Result<int>.Success(42);

        // Act
        var output = result.Match(
            onSuccess: value => $"value {value}",
            onFailure: _ => throw new InvalidOperationException("onFailure must not run on success."));

        // Assert
        output.Should().Be("value 42");
    }

    [Fact]
    public void Match_WhenFailure_CallsOnFailure()
    {
        // Arrange
        var result = Result<int>.Failure(SomeError);

        // Act
        var output = result.Match(
            onSuccess: _ => throw new InvalidOperationException("onSuccess must not run on failure."),
            onFailure: error => $"error {error.Code}");

        // Assert
        output.Should().Be("error code");
    }

    [Fact]
    public void Bind_WhenSuccess_ReturnsResultOfNext()
    {
        // Arrange
        var result = Result<int>.Success(42);

        // Act
        var next = result.Bind(value => Result<string>.Success($"value {value}"));

        // Assert
        next.IsSuccess.Should().BeTrue();
        next.Value.Should().Be("value 42");
    }

    [Fact]
    public void Bind_WhenFailure_DoesNotCallNextAndKeepsError()
    {
        // Arrange
        var result = Result<int>.Failure(SomeError);
        var nextWasCalled = false;

        // Act
        var next = result.Bind(value =>
        {
            nextWasCalled = true;
            return Result<string>.Success($"value {value}");
        });

        // Assert
        nextWasCalled.Should().BeFalse();
        next.IsFailure.Should().BeTrue();
        next.Error.Should().Be(SomeError);
    }

    [Fact]
    public void Map_WhenSuccess_TransformsValue()
    {
        // Arrange
        var result = Result<int>.Success(42);

        // Act
        var mapped = result.Map(value => $"value {value}");

        // Assert
        mapped.IsSuccess.Should().BeTrue();
        mapped.Value.Should().Be("value 42");
    }

    [Fact]
    public void Map_WhenFailure_KeepsErrorWithoutCallingSelector()
    {
        // Arrange
        var result = Result<int>.Failure(SomeError);
        var selectorWasCalled = false;

        // Act
        var mapped = result.Map(value =>
        {
            selectorWasCalled = true;
            return $"value {value}";
        });

        // Assert
        selectorWasCalled.Should().BeFalse();
        mapped.IsFailure.Should().BeTrue();
        mapped.Error.Should().Be(SomeError);
    }
}
