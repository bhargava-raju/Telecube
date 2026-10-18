using MassTransit;
using MediatR;
using Search.Application.Commands.RemoveProduct;
using Telecube.Contracts;

namespace Search.Api.Consumers;

public class ProductDeletedConsumer(ISender sender, ILogger<ProductDeletedConsumer> logger) : IConsumer<ProductDeleted>
{
    public async Task Consume(ConsumeContext<ProductDeleted> context)
    {
        var result = await sender.Send(new RemoveProductCommand(context.Message.ProductId), context.CancellationToken);

        if (result.IsFailure)
            logger.LogWarning("Skipped removing product {ProductId}: {Error}", context.Message.ProductId, result.Error.Message);
    }
}
