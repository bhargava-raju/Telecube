using MassTransit;
using MediatR;
using Search.Application.Commands.IndexProduct;
using Telecube.Contracts;

namespace Search.Api.Consumers;

// Runs each time Catalog publishes ProductCreated. Like a controller, it only turns the message into a command.
public class ProductCreatedConsumer(ISender sender, ILogger<ProductCreatedConsumer> logger) : IConsumer<ProductCreated>
{
    public async Task Consume(ConsumeContext<ProductCreated> context)
    {
        var message = context.Message;
        var command = new IndexProductCommand(message.ProductId, message.CategoryId, message.Sku, message.Name, message.Description, message.Price);
        var result = await sender.Send(command, context.CancellationToken);

        // Retrying an invalid message would fail the same way, so log it and let it go.
        if (result.IsFailure)
            logger.LogWarning("Skipped indexing product {ProductId}: {Error}", message.ProductId, result.Error.Message);
    }
}
