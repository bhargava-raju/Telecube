using Catalog.Application.Abstractions;
using Catalog.Domain;
using MediatR;
using Telecube.BuildingBlocks.Common;

namespace Catalog.Application.Queries.GetProductById;

public class GetProductByIdQueryHandler(IProductRepository productRepository)
    : IRequestHandler<GetProductByIdQuery, Result<Product>>
{
    // TODO: Return the product with the given id, or not-found if there is none.
    public Task<Result<Product>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
