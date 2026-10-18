using MediatR;
using Telecube.BuildingBlocks.Common;

namespace Cart.Application.Commands.RequestCheckout;

public record RequestCheckoutCommand(Guid CustomerId) : IRequest<Result>;
