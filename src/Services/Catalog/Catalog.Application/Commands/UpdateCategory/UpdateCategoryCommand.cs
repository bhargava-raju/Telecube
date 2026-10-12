using MediatR;
using Telecube.BuildingBlocks.Common;

namespace Catalog.Application.Commands.UpdateCategory;

public record UpdateCategoryCommand(Guid Id, string Name, string Description) : IRequest<Result>;
