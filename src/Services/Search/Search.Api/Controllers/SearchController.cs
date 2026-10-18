using MediatR;
using Microsoft.AspNetCore.Mvc;
using Search.Api.Contracts;
using Search.Application.Queries.SearchProducts;
using Search.Domain;
using Telecube.BuildingBlocks.Common;

namespace Search.Api.Controllers;

[ApiController]
[Route("api/search")]
public class SearchController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] SearchProductsRequest request, CancellationToken cancellationToken)
    {
        var query = new SearchProductsQuery(request.Q, request.CategoryId, request.MinPrice, request.MaxPrice, request.Page, request.PageSize);
        var result = await sender.Send(query, cancellationToken);
        return result.Match(
            onSuccess: page => Ok(new ProductSearchResponse(page.Items.Select(ToResponse), page.TotalCount, request.Page, request.PageSize)),
            onFailure: ToErrorResponse);
    }

    private static SearchProductResponse ToResponse(ProductDocument product) =>
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
