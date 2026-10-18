using System.Net;
using System.Net.Http.Json;
using Cart.Application.Abstractions;

namespace Cart.Infrastructure.Clients;

// Adapter over Catalog's REST API. Retries, timeouts and the circuit breaker come from the
// resilience handler added to this HttpClient in InfrastructureExtensions.
public sealed class CatalogClient(HttpClient httpClient) : ICatalogClient
{
    public async Task<CatalogProduct?> GetProductAsync(Guid productId, CancellationToken cancellationToken)
    {
        using var response = await httpClient.GetAsync($"api/products/{productId}", cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();

        var product = await response.Content.ReadFromJsonAsync<CatalogProductResponse>(cancellationToken)
            ?? throw new InvalidOperationException($"Catalog returned an empty body for product {productId}.");

        return new CatalogProduct(product.Id, product.Sku, product.Name, product.Price);
    }

    // Mirrors the fields of Catalog's ProductResponse that Cart reads. Extra fields in the JSON are ignored.
    private sealed record CatalogProductResponse(Guid Id, string Sku, string Name, decimal Price);
}
