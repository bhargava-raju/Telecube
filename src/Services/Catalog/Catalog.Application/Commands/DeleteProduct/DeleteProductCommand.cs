using MediatR;
using Telecube.BuildingBlocks.Common;

namespace Catalog.Application.Commands.DeleteProduct;

public record DeleteProductCommand(Guid Id) : IRequest<Result>;
