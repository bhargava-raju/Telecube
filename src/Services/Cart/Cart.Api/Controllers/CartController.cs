using Cart.Api.Contracts;
using Cart.Application.Commands.AddCartItem;
using Cart.Application.Commands.ChangeCartItemQuantity;
using Cart.Application.Commands.ClearCart;
using Cart.Application.Commands.RemoveCartItem;
using Cart.Application.Commands.RequestCheckout;
using Cart.Application.Queries.GetCart;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using Telecube.BuildingBlocks.Common;
using CartEntity = Cart.Domain.Cart;

namespace Cart.Api.Controllers;

// Every route works on the signed-in shopper's own cart, identified by the token's oid claim,
// so no customer id ever appears in the URL.
[ApiController]
[Route("api/cart")]
[Authorize]
[RequiredScope("Cart.ReadWrite")]
public class CartController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        if (GetCustomerId() is not { } customerId) return Unauthorized();

        var result = await sender.Send(new GetCartQuery(customerId), cancellationToken);
        return result.Match(onSuccess: cart => Ok(ToResponse(cart)), onFailure: ToErrorResponse);
    }

    [HttpPost("items")]
    public async Task<IActionResult> AddItem(AddCartItemRequest request, CancellationToken cancellationToken)
    {
        if (GetCustomerId() is not { } customerId) return Unauthorized();

        var result = await sender.Send(new AddCartItemCommand(customerId, request.ProductId, request.Quantity), cancellationToken);
        return result.Match(onSuccess: cart => Ok(ToResponse(cart)), onFailure: ToErrorResponse);
    }

    [HttpPut("items/{productId:guid}")]
    public async Task<IActionResult> ChangeQuantity(Guid productId, ChangeCartItemQuantityRequest request, CancellationToken cancellationToken)
    {
        if (GetCustomerId() is not { } customerId) return Unauthorized();

        var result = await sender.Send(new ChangeCartItemQuantityCommand(customerId, productId, request.Quantity), cancellationToken);
        return result.Match(onSuccess: cart => Ok(ToResponse(cart)), onFailure: ToErrorResponse);
    }

    [HttpDelete("items/{productId:guid}")]
    public async Task<IActionResult> RemoveItem(Guid productId, CancellationToken cancellationToken)
    {
        if (GetCustomerId() is not { } customerId) return Unauthorized();

        var result = await sender.Send(new RemoveCartItemCommand(customerId, productId), cancellationToken);
        return result.Match(onSuccess: cart => Ok(ToResponse(cart)), onFailure: ToErrorResponse);
    }

    [HttpDelete]
    public async Task<IActionResult> Clear(CancellationToken cancellationToken)
    {
        if (GetCustomerId() is not { } customerId) return Unauthorized();

        var result = await sender.Send(new ClearCartCommand(customerId), cancellationToken);
        return result.IsFailure ? ToErrorResponse(result.Error) : Ok();
    }

    // 202 Accepted: checkout continues asynchronously in the Checkout service once it receives the event.
    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout(CancellationToken cancellationToken)
    {
        if (GetCustomerId() is not { } customerId) return Unauthorized();

        var result = await sender.Send(new RequestCheckoutCommand(customerId), cancellationToken);
        return result.IsFailure ? ToErrorResponse(result.Error) : Accepted();
    }

    private Guid? GetCustomerId()
    {
        var oid = HttpContext.User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;
        return Guid.TryParse(oid, out var customerId) ? customerId : null;
    }

    private static CartResponse ToResponse(CartEntity cart) => new(
        cart.Items.Select(i => new CartItemResponse(i.ProductId, i.Sku, i.Name, i.UnitPrice, i.Quantity, i.LineTotal)),
        cart.Total);

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
