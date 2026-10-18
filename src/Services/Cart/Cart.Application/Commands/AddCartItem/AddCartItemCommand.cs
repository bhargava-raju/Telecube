using MediatR;
using Telecube.BuildingBlocks.Common;
using CartEntity = Cart.Domain.Cart;

namespace Cart.Application.Commands.AddCartItem;

public record AddCartItemCommand(Guid CustomerId, Guid ProductId, int Quantity) : IRequest<Result<CartEntity>>;
