using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Telecube.BuildingBlocks.Observability;

public static class HealthCheckExtensions
{
    // TODO: register liveness/readiness health checks (AddHealthChecks(), tagged "live"/"ready")
    public static IServiceCollection AddDefaultHealthChecks(this IServiceCollection services)
    {
        throw new NotImplementedException();
    }

    // TODO: map /health/live and /health/ready endpoints, filtering by the tags above
    public static IEndpointRouteBuilder MapDefaultHealthChecks(this IEndpointRouteBuilder endpoints)
    {
        throw new NotImplementedException();
    }
}
