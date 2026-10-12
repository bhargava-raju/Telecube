using Catalog.Domain;
using MediatR;

namespace Catalog.Application.Queries.GetCategories;

public record GetCategoriesQuery : IRequest<IReadOnlyList<Category>>;
