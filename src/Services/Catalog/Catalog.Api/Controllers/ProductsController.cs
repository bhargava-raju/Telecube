using Catalog.Api.Contracts;
using Catalog.Application.Commands.CreateProduct;
using Catalog.Application.Commands.DeleteProduct;
using Catalog.Application.Commands.UpdateProduct;
using Catalog.Application.Queries.GetProductById;
using Catalog.Application.Queries.GetProducts;
using Catalog.Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using Telecube.BuildingBlocks.Common;

namespace Catalog.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController(ISender sender) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var products = await sender.Send(new GetProductsQuery(), cancellationToken);
        return Ok(products.Select(ToResponse));
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetProductByIdQuery(id), cancellationToken);
        return result.Match(
            onSuccess: product => Ok(ToResponse(product)),
            onFailure: ToErrorResponse);
    }

    [HttpPost]
    [Authorize]
    [RequiredScope("Catalog.Write")]
    public async Task<IActionResult> Create(CreateProductRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateProductCommand(request.CategoryId, request.Sku, request.Name, request.Description, request.Price);
        var result = await sender.Send(command, cancellationToken);
        return result.Match(
            onSuccess: product => CreatedAtAction(nameof(GetById), new { id = product.Id }, ToResponse(product)),
            onFailure: ToErrorResponse);
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    [RequiredScope("Catalog.Write")]
    public async Task<IActionResult> Update(Guid id, UpdateProductRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateProductCommand(id, request.CategoryId, request.Name, request.Description, request.Price);
        var result = await sender.Send(command, cancellationToken);
        return result.IsFailure ? ToErrorResponse(result.Error) : Ok();
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    [RequiredScope("Catalog.Write")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeleteProductCommand(id), cancellationToken);
        return result.IsFailure ? ToErrorResponse(result.Error) : Ok();
    }

    private static ProductResponse ToResponse(Product product) =>
        new(product.Id, product.CategoryId, product.Sku, product.Name, product.Description, product.Price);

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
