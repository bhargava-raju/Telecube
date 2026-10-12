using Catalog.Application.Abstractions;
using Catalog.Domain;
using MediatR;

namespace Catalog.Application.Queries.GetProducts;

public class GetProductsQueryHandler(IProductRepository productRepository)
    : IRequestHandler<GetProductsQuery, IReadOnlyList<Product>>
{
    public async Task<IReadOnlyList<Product>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await productRepository.GetAllAsync(cancellationToken);
        return products.ToList();
    }
}
