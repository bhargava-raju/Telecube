using Catalog.Domain;
using MediatR;
using Telecube.BuildingBlocks.Common;

namespace Catalog.Application.Queries.GetProductById;

public record GetProductByIdQuery(Guid Id) : IRequest<Result<Product>>;
