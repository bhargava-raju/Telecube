using MassTransit;
using MediatR;
using Search.Application.Commands.IndexProduct;
using Telecube.Contracts;

namespace Search.Api.Consumers;

// An update re-indexes the whole product, so it sends the same command as a create.
public class ProductUpdatedConsumer(ISender sender, ILogger<ProductUpdatedConsumer> logger) : IConsumer<ProductUpdated>
{
    public async Task Consume(ConsumeContext<ProductUpdated> context)
    {
        var message = context.Message;
        var command = new IndexProductCommand(message.ProductId, message.CategoryId, message.Sku, message.Name, message.Description, message.Price);
        var result = await sender.Send(command, context.CancellationToken);

        if (result.IsFailure)
            logger.LogWarning("Skipped re-indexing product {ProductId}: {Error}", message.ProductId, result.Error.Message);
    }
}
