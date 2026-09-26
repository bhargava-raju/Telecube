using Microsoft.Extensions.DependencyInjection;

namespace Telecube.BuildingBlocks.Web;

public static class ProblemDetailsExtensions
{
    public static IServiceCollection AddDefaultProblemDetails(this IServiceCollection services)
    {
        return services.AddProblemDetails();
    }
}
