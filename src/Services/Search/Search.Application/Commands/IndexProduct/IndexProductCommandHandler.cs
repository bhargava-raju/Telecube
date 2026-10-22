using MediatR;
using Search.Application.Abstractions;
using Search.Domain;
using Telecube.BuildingBlocks.Common;

namespace Search.Application.Commands.IndexProduct;

public class IndexProductCommandHandler(IProductSearchIndex searchIndex)
    : IRequestHandler<IndexProductCommand, Result>
{
    // TODO: Build a search document from the command and add it to the index, replacing any existing one, or return the validation failure.
    public async Task<Result> Handle(IndexProductCommand request, CancellationToken cancellationToken)
    {
        var result = ProductDocument.Create(request.ProductId, request.CategoryId, request.Sku, request.Name, request.Description, request.Price);
        if (result.IsSuccess)
        { 
            await searchIndex.UpsertAsync(result.Value, cancellationToken);
            return Result.Success();
        }
        return Result.Failure(result.Error);
    }
}
