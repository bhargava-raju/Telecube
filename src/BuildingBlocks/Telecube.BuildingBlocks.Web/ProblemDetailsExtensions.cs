using Microsoft.Extensions.DependencyInjection;

namespace Telecube.BuildingBlocks.Web;

public static class ProblemDetailsExtensions
{
    // TODO: AddProblemDetails() + a custom IExceptionHandler that never leaks stack traces,
    // and maps Result/Error (from BuildingBlocks.Common) to the right HTTP status code.
    public static IServiceCollection AddDefaultProblemDetails(this IServiceCollection services)
    {
        throw new NotImplementedException();
    }
}
