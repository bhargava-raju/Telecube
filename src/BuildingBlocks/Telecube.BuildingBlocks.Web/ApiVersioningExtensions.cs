using Microsoft.Extensions.DependencyInjection;

namespace Telecube.BuildingBlocks.Web;

public static class ApiVersioningExtensions
{
    // TODO: AddApiVersioning() + AddApiExplorer() with this project's chosen versioning scheme
    // (URL segment vs header vs query string).
    public static IServiceCollection AddDefaultApiVersioning(this IServiceCollection services)
    {
        throw new NotImplementedException();
    }
}
