using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Microsoft.Extensions.Hosting;

namespace Search.Infrastructure.Indexing;

// Creates the index on startup, or updates its schema if ProductIndexDocument gained fields.
public sealed class SearchIndexInitializer(SearchIndexClient indexClient, AzureSearchOptions options) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var fields = new FieldBuilder().Build(typeof(ProductIndexDocument));
        await indexClient.CreateOrUpdateIndexAsync(new SearchIndex(options.IndexName, fields), cancellationToken: cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
