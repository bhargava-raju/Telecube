using Catalog.Application.Abstractions;
using Catalog.Domain;
using MediatR;
using Telecube.BuildingBlocks.Common;

namespace Catalog.Application.Queries.GetProductById;

public class GetProductByIdQueryHandler(IProductRepository productRepository)
    : IRequestHandler<GetProductByIdQuery, Result<Product>>
{
    public async Task<Result<Product>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(request.Id, cancellationToken);
        if (product == null)
        {
            return Result<Product>.Failure(new Error("NotFound", $"Product with id {request.Id} not found", ErrorType.NotFound));
        }

        return Result<Product>.Success(product);
    }
}
