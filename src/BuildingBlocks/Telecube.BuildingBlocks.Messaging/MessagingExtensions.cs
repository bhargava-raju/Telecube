using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Telecube.BuildingBlocks.Messaging;

public static class MessagingExtensions
{
    // TODO: AddMassTransit(...) wired to the Azure Service Bus transport (host connection string
    // from config), with consumers registered per-service by whoever calls this.
    public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        throw new NotImplementedException();
    }
}
