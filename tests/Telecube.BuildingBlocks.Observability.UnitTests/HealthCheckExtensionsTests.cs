namespace Telecube.BuildingBlocks.Observability.UnitTests;

public class HealthCheckExtensionsTests
{
    // TODO once implemented: AddDefaultHealthChecks registers checks tagged "live"/"ready";
    // MapDefaultHealthChecks exposes /health/live and /health/ready, each filtering by its tag.
    // Likely wants Microsoft.AspNetCore.Mvc.Testing + WebApplicationFactory against a minimal
    // test host, similar to Telecube.Gateway.IntegrationTests.
    [Fact]
    public void Placeholder()
    {
        Assert.True(true);
    }
}
