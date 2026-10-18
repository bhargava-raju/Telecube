using MediatR;
using Telecube.BuildingBlocks.Common;
using CartEntity = Cart.Domain.Cart;

namespace Cart.Application.Queries.GetCart;

public record GetCartQuery(Guid CustomerId) : IRequest<Result<CartEntity>>;
