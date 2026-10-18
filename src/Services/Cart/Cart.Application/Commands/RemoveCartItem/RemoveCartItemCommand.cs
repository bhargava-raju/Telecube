using MediatR;
using Telecube.BuildingBlocks.Common;
using CartEntity = Cart.Domain.Cart;

namespace Cart.Application.Commands.RemoveCartItem;

public record RemoveCartItemCommand(Guid CustomerId, Guid ProductId) : IRequest<Result<CartEntity>>;
