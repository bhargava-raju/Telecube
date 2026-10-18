using CartEntity = Cart.Domain.Cart;

namespace Cart.Application.Abstractions;

public interface ICartRepository
{
    Task<CartEntity?> GetAsync(Guid customerId, CancellationToken cancellationToken);
    Task SaveAsync(CartEntity cart, CancellationToken cancellationToken);
    Task DeleteAsync(Guid customerId, CancellationToken cancellationToken);
}
