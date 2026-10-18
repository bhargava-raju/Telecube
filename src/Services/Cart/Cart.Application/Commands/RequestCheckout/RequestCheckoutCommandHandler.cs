using Cart.Application.Abstractions;
using MediatR;
using Telecube.BuildingBlocks.Common;

namespace Cart.Application.Commands.RequestCheckout;

public class RequestCheckoutCommandHandler(ICartRepository cartRepository, IIntegrationEventPublisher integrationEventPublisher)
    : IRequestHandler<RequestCheckoutCommand, Result>
{
    // TODO: Return a validation failure if the customer has no cart or it is empty, otherwise publish CheckoutRequested with the cart's items and total and return success.
    public Task<Result> Handle(RequestCheckoutCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
