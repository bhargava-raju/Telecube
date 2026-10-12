using Catalog.Application.Abstractions;
using MediatR;
using Telecube.BuildingBlocks.Common;

namespace Catalog.Application.Commands.DeleteCategory;

public class DeleteCategoryCommandHandler(ICategoryRepository categoryRepository)
    : IRequestHandler<DeleteCategoryCommand, Result>
{
    // TODO: Return not-found if the category is missing, a conflict if it still has products, otherwise delete it.
    public Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
