using MediatR;
using Telecube.BuildingBlocks.Common;
using CartEntity = Cart.Domain.Cart;

namespace Cart.Application.Commands.ChangeCartItemQuantity;

public record ChangeCartItemQuantityCommand(Guid CustomerId, Guid ProductId, int Quantity) : IRequest<Result<CartEntity>>;
