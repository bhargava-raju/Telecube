using Cart.Application.Abstractions;
using MediatR;
using Telecube.BuildingBlocks.Common;
using CartEntity = Cart.Domain.Cart;

namespace Cart.Application.Commands.ChangeCartItemQuantity;

public class ChangeCartItemQuantityCommandHandler(ICartRepository cartRepository)
    : IRequestHandler<ChangeCartItemQuantityCommand, Result<CartEntity>>
{
    // TODO: Return not-found if the customer has no cart, otherwise change the item's quantity, save the cart, and return it, or the failure.
    public Task<Result<CartEntity>> Handle(ChangeCartItemQuantityCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
