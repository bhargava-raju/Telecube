using Catalog.Domain;
using MediatR;
using Telecube.BuildingBlocks.Common;

namespace Catalog.Application.Queries.GetCategoryById;

public record GetCategoryByIdQuery(Guid Id) : IRequest<Result<Category>>;
