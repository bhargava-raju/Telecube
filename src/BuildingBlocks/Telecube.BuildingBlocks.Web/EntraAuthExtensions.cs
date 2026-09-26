using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Telecube.BuildingBlocks.Web;

public static class EntraAuthExtensions
{
    // TODO: AddMicrosoftIdentityWebApiAuthentication(configuration) so every service validates
    // Entra-issued JWTs itself (defense in depth — see the design doc's Security section).
    public static IServiceCollection AddEntraAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        throw new NotImplementedException();
    }
}
