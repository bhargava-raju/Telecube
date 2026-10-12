using Catalog.Application.Abstractions;
using MediatR;
using Telecube.BuildingBlocks.Common;

namespace Catalog.Application.Commands.UpdateProduct;

public class UpdateProductCommandHandler(IProductRepository productRepository, ICategoryRepository categoryRepository, IIntegrationEventPublisher integrationEventPublisher)
    : IRequestHandler<UpdateProductCommand, Result>
{
    // TODO: Return not-found if the product or category is missing, otherwise apply the changes, save them, and publish ProductUpdated.
    public Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
