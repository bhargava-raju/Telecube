using Azure;
using Azure.Identity;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Search.Application.Abstractions;
using Search.Infrastructure.Indexing;

namespace Search.Infrastructure;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddSearchInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var endpoint = configuration["AzureSearch:Endpoint"]
            ?? throw new InvalidOperationException("Missing setting 'AzureSearch:Endpoint'.");
        var indexName = configuration["AzureSearch:IndexName"] ?? "products";
        var apiKey = configuration["AzureSearch:ApiKey"];

        var options = new AzureSearchOptions(new Uri(endpoint), indexName, apiKey);
        services.AddSingleton(options);

        // Search clients are thread-safe and meant to be reused, so one instance each for the app's lifetime.
        if (string.IsNullOrEmpty(options.ApiKey))
        {
            var credential = new DefaultAzureCredential();
            services.AddSingleton(new SearchIndexClient(options.Endpoint, credential));
            services.AddSingleton(new SearchClient(options.Endpoint, options.IndexName, credential));
        }
        else
        {
            var credential = new AzureKeyCredential(options.ApiKey);
            services.AddSingleton(new SearchIndexClient(options.Endpoint, credential));
            services.AddSingleton(new SearchClient(options.Endpoint, options.IndexName, credential));
        }

        services.AddHostedService<SearchIndexInitializer>();
        services.AddScoped<IProductSearchIndex, AzureProductSearchIndex>();

        return services;
    }
}
