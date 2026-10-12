using Catalog.Application.Abstractions;
using Catalog.Domain;
using MediatR;
using Telecube.BuildingBlocks.Common;

namespace Catalog.Application.Commands.CreateProduct;

public class CreateProductCommandHandler(IProductRepository productRepository, ICategoryRepository categoryRepository, IIntegrationEventPublisher integrationEventPublisher)
    : IRequestHandler<CreateProductCommand, Result<Product>>
{
    // TODO: Reject an unknown category as not-found and a duplicate SKU as a conflict, otherwise create and save the product, publish ProductCreated, and return it.
    public Task<Result<Product>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
