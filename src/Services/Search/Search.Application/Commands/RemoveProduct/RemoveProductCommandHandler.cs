using MediatR;
using Search.Application.Abstractions;
using Telecube.BuildingBlocks.Common;

namespace Search.Application.Commands.RemoveProduct;

public class RemoveProductCommandHandler(IProductSearchIndex searchIndex)
    : IRequestHandler<RemoveProductCommand, Result>
{
    public async Task<Result> Handle(RemoveProductCommand request, CancellationToken cancellationToken)
    {
        await searchIndex.DeleteAsync(request.ProductId, cancellationToken);
        return Result.Success();
    }
}
