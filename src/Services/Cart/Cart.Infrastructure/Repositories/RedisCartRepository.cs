using System.Text.Json;
using Cart.Application.Abstractions;
using StackExchange.Redis;
using CartEntity = Cart.Domain.Cart;

namespace Cart.Infrastructure.Repositories;

// Each cart is one Redis string holding JSON, under the key "cart:{customerId}".
public sealed class RedisCartRepository(IConnectionMultiplexer redis) : ICartRepository
{
    // Abandoned carts expire on their own. Every save restarts the clock.
    private static readonly TimeSpan CartLifetime = TimeSpan.FromDays(30);

    public async Task<CartEntity?> GetAsync(Guid customerId, CancellationToken cancellationToken)
    {
        var json = await redis.GetDatabase().StringGetAsync(Key(customerId));
        if (json.IsNullOrEmpty)
            return null;

        var document = JsonSerializer.Deserialize<CartDocument>(json.ToString())
            ?? throw new InvalidOperationException($"Cart for customer {customerId} could not be read.");

        return ToDomain(document);
    }

    public async Task SaveAsync(CartEntity cart, CancellationToken cancellationToken)
    {
        var json = JsonSerializer.Serialize(ToDocument(cart));
        await redis.GetDatabase().StringSetAsync(Key(cart.CustomerId), json, CartLifetime);
    }

    public async Task DeleteAsync(Guid customerId, CancellationToken cancellationToken)
    {
        await redis.GetDatabase().KeyDeleteAsync(Key(customerId));
    }

    private static RedisKey Key(Guid customerId) => $"cart:{customerId}";

    private static CartDocument ToDocument(CartEntity cart) => new(
        cart.CustomerId,
        cart.Items.Select(i => new CartItemDocument(i.ProductId, i.Sku, i.Name, i.UnitPrice, i.Quantity)).ToList());

    // Rebuilds the cart through its own methods, so stored data passes the same rules as new data.
    private static CartEntity ToDomain(CartDocument document)
    {
        var cart = CartEntity.Create(document.CustomerId);
        if (cart.IsFailure)
            throw new InvalidOperationException($"Stored cart is invalid: {cart.Error.Message}");

        foreach (var item in document.Items)
        {
            var added = cart.Value.AddItem(item.ProductId, item.Sku, item.Name, item.UnitPrice, item.Quantity);
            if (added.IsFailure)
                throw new InvalidOperationException($"Stored cart {document.CustomerId} is invalid: {added.Error.Message}");
        }

        return cart.Value;
    }

    // Storage shapes, kept separate from the domain so the domain needs no serializer attributes.
    private sealed record CartDocument(Guid CustomerId, List<CartItemDocument> Items);

    private sealed record CartItemDocument(Guid ProductId, string Sku, string Name, decimal UnitPrice, int Quantity);
}
