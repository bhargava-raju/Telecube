using MediatR;
using Telecube.BuildingBlocks.Common;

namespace Catalog.Application.Commands.UpdateProduct;

public record UpdateProductCommand(Guid Id, string Name, string Description, decimal Price)
    : IRequest<Result>;
