using MediatR;
using Telecube.BuildingBlocks.Common;

namespace Search.Application.Commands.RemoveProduct;

public record RemoveProductCommand(Guid ProductId) : IRequest<Result>;
