using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Telecube.BuildingBlocks.Web;

public static class KeyVaultExtensions
{
    // TODO: builder.Configuration.AddAzureKeyVault(...) using DefaultAzureCredential (managed
    // identity in Azure, developer credentials locally) so every service reads secrets/connection
    // strings the same way — see infra/bicep/keyvault.bicep for the matching resource.
    public static IHostApplicationBuilder AddKeyVaultConfiguration(this IHostApplicationBuilder builder)
    {
        throw new NotImplementedException();
    }
}
