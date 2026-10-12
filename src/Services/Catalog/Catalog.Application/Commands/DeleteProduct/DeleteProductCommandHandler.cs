using Catalog.Application.Abstractions;
using MediatR;
using Telecube.BuildingBlocks.Common;

namespace Catalog.Application.Commands.DeleteProduct;

public class DeleteProductCommandHandler(IProductRepository productRepository, IIntegrationEventPublisher integrationEventPublisher)
    : IRequestHandler<DeleteProductCommand, Result>
{
    // TODO: Return not-found if the product is missing, otherwise delete it and publish ProductDeleted.
    public Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
