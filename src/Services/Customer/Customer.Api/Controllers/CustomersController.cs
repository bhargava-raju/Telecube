using Customer.Api.Contracts;
using Customer.Application.Commands.CreateCustomer;
using Customer.Application.Commands.UpdateCustomerProfile;
using Customer.Application.Queries.GetCustomerByEntraId;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using Telecube.BuildingBlocks.Common;

namespace Customer.Api.Controllers;

[ApiController]
[Route("api/customers")]
[Authorize]
[RequiredScope("Customer.ReadWrite")]
public class CustomersController(ISender sender) : ControllerBase
{
    [HttpGet("GetProfile")]    
    public async Task<IActionResult> GetProfile(CancellationToken cancellationToken)
    {
        var oid = GetEntraObjectId();
        if (oid == null)  return Unauthorized();

        var result = await sender.Send(new GetCustomerByEntraIdQuery(oid), cancellationToken);

        return result.Match(
            onSuccess: customer => Ok(new CustomerResponse(customer.Email, customer.DisplayName)),
            onFailure: ToErrorResponse);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> CreateProfile(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        var oid = GetEntraObjectId();
        if (oid == null) return Unauthorized();

        var result = await sender.Send(new CreateCustomerCommand(oid, request.Email, request.DisplayName), cancellationToken);

        return result.Match(
            onSuccess: _ => Ok(),
            onFailure: ToErrorResponse);

    }

    [HttpPut("Update")]
    public async Task<IActionResult> UpdateProfile(UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        var oid = GetEntraObjectId();
        if (oid == null) return Unauthorized();

        var result = await sender.Send(new UpdateCustomerProfileCommand(oid, request.Email, request.DisplayName), cancellationToken);
        return result.IsFailure ? ToErrorResponse(result.Error) : Ok();
    }

    private string? GetEntraObjectId()
    {
        return HttpContext.User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;
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
