using Microsoft.AspNetCore.Mvc.Testing;

namespace Telecube.Gateway.IntegrationTests;

public class GatewayTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public GatewayTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    // TODO once HealthCheckExtensions is implemented and wired into Gateway:
    // GET /health/live and /health/ready both return 200.
    // TODO once WeatherForecast is swapped for a real placeholder endpoint: test that instead.
    [Fact]
    public void Placeholder()
    {
        Assert.True(true);
    }
}
