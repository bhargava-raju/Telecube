using Catalog.Application.Abstractions;
using Catalog.Domain;
using MediatR;
using Telecube.BuildingBlocks.Common;
using Telecube.Contracts;

namespace Catalog.Application.Commands.CreateProduct;

public class CreateProductCommandHandler(IProductRepository productRepository, ICategoryRepository categoryRepository, IIntegrationEventPublisher integrationEventPublisher)
    : IRequestHandler<CreateProductCommand, Result<Product>>
{
    public async Task<Result<Product>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var newProduct = Product.Create(request.CategoryId, request.Sku, request.Name, request.Description, request.Price);
        if (newProduct.IsFailure)
        {
            return Result<Product>.Failure(newProduct.Error);
        }

        var category = await categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
        if (category == null)
        {
            return Result<Product>.Failure(new Error("NotFound", $"Category with id {request.CategoryId} not found", ErrorType.NotFound));
        }

        var existingProduct = await productRepository.GetBySkuAsync(request.Sku, cancellationToken);
        if (existingProduct != null)
        {
            return Result<Product>.Failure(new Error("Conflict", $"Product with SKU {request.Sku} already exists", ErrorType.Conflict));
        }

        await productRepository.AddAsync(newProduct.Value, cancellationToken);

        var productCreatedEvent = new ProductCreated(
            newProduct.Value.Id,
            newProduct.Value.CategoryId,
            newProduct.Value.Sku,
            newProduct.Value.Name,
            newProduct.Value.Description,
            newProduct.Value.Price);

        await integrationEventPublisher.PublishAsync(productCreatedEvent, cancellationToken);

        return Result<Product>.Success(newProduct.Value);
    }
}
