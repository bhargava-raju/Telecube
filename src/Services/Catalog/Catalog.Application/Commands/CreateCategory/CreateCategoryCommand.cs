using Catalog.Domain;
using MediatR;
using Telecube.BuildingBlocks.Common;

namespace Catalog.Application.Commands.CreateCategory;

public record CreateCategoryCommand(string Name, string Description) : IRequest<Result<Category>>;
