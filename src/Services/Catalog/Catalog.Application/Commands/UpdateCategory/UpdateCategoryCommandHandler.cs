using Catalog.Application.Abstractions;
using MediatR;
using Telecube.BuildingBlocks.Common;

namespace Catalog.Application.Commands.UpdateCategory;

public class UpdateCategoryCommandHandler(ICategoryRepository categoryRepository)
    : IRequestHandler<UpdateCategoryCommand, Result>
{
    // TODO: Return not-found if the category is missing, otherwise apply the changes and save them.
    public Task<Result> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
