namespace Search.Infrastructure;

// ApiKey is for local development only. When it's empty the service signs in with its managed identity.
public sealed record AzureSearchOptions(Uri Endpoint, string IndexName, string? ApiKey);
