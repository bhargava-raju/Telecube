using Catalog.Application.Abstractions;
using MediatR;
using Telecube.BuildingBlocks.Common;
using Telecube.Contracts;

namespace Catalog.Application.Commands.UpdateProduct;

public class UpdateProductCommandHandler(IProductRepository productRepository, ICategoryRepository categoryRepository, IIntegrationEventPublisher integrationEventPublisher)
    : IRequestHandler<UpdateProductCommand, Result>
{
    public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(request.Id, cancellationToken);
        if (product == null)
        {
            return Result.Failure(new Error("NotFound", $"Product with id {request.Id} not found", ErrorType.NotFound));
        }

        var category = await categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
        if (category == null)
        {
            return Result.Failure(new Error("NotFound", $"Category with id {request.CategoryId} not found", ErrorType.NotFound));
        }

        var result = product.Update(request.CategoryId, request.Name, request.Description, request.Price);
        if (result.IsFailure)
        {
            return result;
        }

        await productRepository.UpdateAsync(product, cancellationToken);

        var productUpdatedEvent = new ProductUpdated(
            product.Id,
            product.CategoryId,
            product.Sku,
            product.Name,
            product.Description,
            product.Price);

        await integrationEventPublisher.PublishAsync(productUpdatedEvent, cancellationToken);

        return Result.Success();
    }
}
