using Catalog.Domain;
using MediatR;

namespace Catalog.Application.Queries.GetProducts;

public record GetProductsQuery : IRequest<IReadOnlyList<Product>>;
