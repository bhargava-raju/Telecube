using MediatR;
using Telecube.BuildingBlocks.Common;

namespace Search.Application.Commands.IndexProduct;

public record IndexProductCommand(Guid ProductId, Guid CategoryId, string Sku, string Name, string Description, decimal Price)
    : IRequest<Result>;
