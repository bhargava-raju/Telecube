using MediatR;
using Search.Application.Abstractions;
using Telecube.BuildingBlocks.Common;

namespace Search.Application.Commands.IndexProduct;

public class IndexProductCommandHandler(IProductSearchIndex searchIndex)
    : IRequestHandler<IndexProductCommand, Result>
{
    // TODO: Build a search document from the command and add it to the index, replacing any existing one, or return the validation failure.
    public Task<Result> Handle(IndexProductCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
