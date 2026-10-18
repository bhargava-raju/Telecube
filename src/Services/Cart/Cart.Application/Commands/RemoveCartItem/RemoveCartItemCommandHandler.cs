using Cart.Application.Abstractions;
using MediatR;
using Telecube.BuildingBlocks.Common;
using CartEntity = Cart.Domain.Cart;

namespace Cart.Application.Commands.RemoveCartItem;

public class RemoveCartItemCommandHandler(ICartRepository cartRepository)
    : IRequestHandler<RemoveCartItemCommand, Result<CartEntity>>
{
    // TODO: Return not-found if the customer has no cart, otherwise remove the item, save the cart, and return it, or the failure.
    public Task<Result<CartEntity>> Handle(RemoveCartItemCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
