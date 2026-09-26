namespace Telecube.BuildingBlocks.Common.UnitTests;

public class ResultTests
{
    // TODO once Result/Result<T> is implemented:
    // - Success()/Failure() set IsSuccess/IsFailure correctly and are mutually exclusive.
    // - Accessing Value on a failed Result<T> doesn't silently return default.
    // - Match forces both branches; Bind short-circuits on the first failure without
    //   evaluating the next step.
    [Fact]
    public void Placeholder()
    {
        Assert.True(true);
    }
}
