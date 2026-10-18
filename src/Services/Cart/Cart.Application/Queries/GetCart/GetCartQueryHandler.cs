using Cart.Application.Abstractions;
using MediatR;
using Telecube.BuildingBlocks.Common;
using CartEntity = Cart.Domain.Cart;

namespace Cart.Application.Queries.GetCart;

public class GetCartQueryHandler(ICartRepository cartRepository)
    : IRequestHandler<GetCartQuery, Result<CartEntity>>
{
    // TODO: Return the customer's saved cart, or a new empty cart if none is saved.
    public Task<Result<CartEntity>> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
