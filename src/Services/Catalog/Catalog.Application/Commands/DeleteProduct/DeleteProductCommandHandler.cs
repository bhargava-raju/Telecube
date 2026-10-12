using Catalog.Application.Abstractions;
using MediatR;
using Telecube.BuildingBlocks.Common;
using Telecube.Contracts;

namespace Catalog.Application.Commands.DeleteProduct;

public class DeleteProductCommandHandler(IProductRepository productRepository, IIntegrationEventPublisher integrationEventPublisher)
    : IRequestHandler<DeleteProductCommand, Result>
{
    public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(request.Id, cancellationToken);
        if (product == null)
        {
            return Result.Failure(new Error("NotFound", $"Product with id {request.Id} not found", ErrorType.NotFound));
        }

        await productRepository.DeleteAsync(product.Id, cancellationToken);
        await integrationEventPublisher.PublishAsync(new ProductDeleted(product.Id), cancellationToken);

        return Result.Success();
    }
}
