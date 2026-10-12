using Catalog.Domain;
using MediatR;
using Telecube.BuildingBlocks.Common;

namespace Catalog.Application.Queries.GetCategories;

public record GetCategoriesQuery : IRequest<Result<IReadOnlyList<Category>>>;
