using MediatR;
using Telecube.BuildingBlocks.Common;

namespace Catalog.Application.Commands.UpdateProduct;

public record UpdateProductCommand(Guid Id, Guid CategoryId, string Name, string Description, decimal Price)
    : IRequest<Result>;
