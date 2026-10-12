using MediatR;
using Telecube.BuildingBlocks.Common;

namespace Catalog.Application.Commands.DeleteCategory;

public record DeleteCategoryCommand(Guid Id) : IRequest<Result>;
