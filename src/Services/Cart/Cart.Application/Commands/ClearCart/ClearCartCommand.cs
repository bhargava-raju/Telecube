using MediatR;
using Telecube.BuildingBlocks.Common;

namespace Cart.Application.Commands.ClearCart;

public record ClearCartCommand(Guid CustomerId) : IRequest<Result>;
