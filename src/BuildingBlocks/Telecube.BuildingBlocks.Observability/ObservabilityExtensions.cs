using Microsoft.Extensions.Hosting;

namespace Telecube.BuildingBlocks.Observability;

public static class ObservabilityExtensions
{
    // TODO: wire Serilog (reading sinks/config from appsettings) + OpenTelemetry tracing/metrics
    // (AspNetCore + Http instrumentation, OTLP exporter) onto the host builder.
    public static IHostApplicationBuilder AddObservability(this IHostApplicationBuilder builder)
    {
        throw new NotImplementedException();
    }
}
