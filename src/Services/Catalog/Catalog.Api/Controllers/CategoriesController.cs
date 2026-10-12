using Catalog.Api.Contracts;
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
    // TODO: Return every category as a response list.
    [HttpGet]
    [AllowAnonymous]
    public Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    // TODO: Return the category with the given id, or the matching error response.
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    // TODO: Create the category from the request and return it, or the matching error response.
    [HttpPost]
    [Authorize]
    [RequiredScope("Catalog.Write")]
    public Task<IActionResult> Create(CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    // TODO: Update the category with the given id from the request, or return the matching error response.
    [HttpPut("{id:guid}")]
    [Authorize]
    [RequiredScope("Catalog.Write")]
    public Task<IActionResult> Update(Guid id, UpdateCategoryRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    // TODO: Delete the category with the given id, or return the matching error response.
    [HttpDelete("{id:guid}")]
    [Authorize]
    [RequiredScope("Catalog.Write")]
    public Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    // TODO: Turn an error into a problem response with the HTTP status that fits its type.
    private IActionResult ToErrorResponse(Error error)
    {
        throw new NotImplementedException();
    }
}
