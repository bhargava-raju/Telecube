using Catalog.Application.Abstractions;
using Catalog.Domain;
using MediatR;

namespace Catalog.Application.Queries.GetProducts;

public class GetProductsQueryHandler(IProductRepository productRepository)
    : IRequestHandler<GetProductsQuery, IReadOnlyList<Product>>
{
    // TODO: Return every product in the catalog.
    public Task<IReadOnlyList<Product>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
