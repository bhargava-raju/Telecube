using MediatR;
using Search.Application.Abstractions;
using Telecube.BuildingBlocks.Common;

namespace Search.Application.Commands.RemoveProduct;

public class RemoveProductCommandHandler(IProductSearchIndex searchIndex)
    : IRequestHandler<RemoveProductCommand, Result>
{
    // TODO: Remove the product with the given id from the index and return success.
    public Task<Result> Handle(RemoveProductCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
