using Cart.Application.Abstractions;
using MediatR;
using Telecube.BuildingBlocks.Common;
using CartEntity = Cart.Domain.Cart;

namespace Cart.Application.Commands.AddCartItem;

public class AddCartItemCommandHandler(ICartRepository cartRepository, ICatalogClient catalogClient)
    : IRequestHandler<AddCartItemCommand, Result<CartEntity>>
{
    // TODO: Return not-found if the catalog has no such product, otherwise add it to the customer's cart (starting a new cart if none exists) with the catalog's current SKU, name and price, save the cart, and return it.
    public Task<Result<CartEntity>> Handle(AddCartItemCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
