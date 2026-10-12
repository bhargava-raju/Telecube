using Catalog.Domain;
using MediatR;
using Telecube.BuildingBlocks.Common;

namespace Catalog.Application.Commands.CreateProduct;

public record CreateProductCommand(string Sku, string Name, string Description, decimal Price)
    : IRequest<Result<Product>>;
