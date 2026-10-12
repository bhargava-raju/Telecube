using Catalog.Api.Contracts;
using Catalog.Application.Commands.CreateCategory;
using Catalog.Application.Commands.DeleteCategory;
using Catalog.Application.Commands.UpdateCategory;
using Catalog.Application.Queries.GetCategories;
using Catalog.Application.Queries.GetCategoryById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using Telecube.BuildingBlocks.Common;

namespace Catalog.Api.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController(ISender sender) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetCategoriesQuery(), cancellationToken);
        return result.Match(
            onSuccess: categories => Ok(categories.Select(c => new CategoryResponse(c.Id, c.Name, c.Description))),
            onFailure: ToErrorResponse);
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetCategoryByIdQuery(id), cancellationToken);
        return result.Match(
            onSuccess: category => Ok(new CategoryResponse(category.Id, category.Name, category.Description)),
            onFailure: ToErrorResponse);
    }

    [HttpPost]
    [Authorize]
    [RequiredScope("Catalog.Write")]
    public async Task<IActionResult> Create(CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new CreateCategoryCommand(request.Name, request.Description), cancellationToken);
        return result.Match(
            onSuccess: category => CreatedAtAction(nameof(GetById), new { id = category.Id }, new CategoryResponse(category.Id, category.Name, category.Description)),
            onFailure: ToErrorResponse);
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    [RequiredScope("Catalog.Write")]
    public async Task<IActionResult> Update(Guid id, UpdateCategoryRequest request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new UpdateCategoryCommand(id, request.Name, request.Description), cancellationToken);
        return result.IsFailure ? ToErrorResponse(result.Error) : Ok();
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    [RequiredScope("Catalog.Write")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeleteCategoryCommand(id), cancellationToken);
        return result.IsFailure ? ToErrorResponse(result.Error) : Ok();
    }

    private IActionResult ToErrorResponse(Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        return Problem(title: error.Code, detail: error.Message, statusCode: statusCode);
    }
}
