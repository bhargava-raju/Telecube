using Catalog.Application.Abstractions;
using Catalog.Domain;
using MediatR;
using Telecube.BuildingBlocks.Common;

namespace Catalog.Application.Commands.CreateCategory;

public class CreateCategoryCommandHandler(ICategoryRepository categoryRepository)
    : IRequestHandler<CreateCategoryCommand, Result<Category>>
{
    // TODO: Reject a duplicate category name as a conflict, otherwise create and save the category and return it.
    public Task<Result<Category>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
