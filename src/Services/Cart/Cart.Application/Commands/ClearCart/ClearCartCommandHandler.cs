using Cart.Application.Abstractions;
using MediatR;
using Telecube.BuildingBlocks.Common;

namespace Cart.Application.Commands.ClearCart;

public class ClearCartCommandHandler(ICartRepository cartRepository)
    : IRequestHandler<ClearCartCommand, Result>
{
    // TODO: Delete the customer's saved cart and return success.
    public Task<Result> Handle(ClearCartCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
