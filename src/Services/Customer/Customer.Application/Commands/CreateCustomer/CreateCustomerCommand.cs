using MediatR;
using Telecube.BuildingBlocks.Common;
using CustomerEntity = Customer.Domain.Customer;

namespace Customer.Application.Commands.CreateCustomer;

public record CreateCustomerCommand(string EntraObjectId, string Email, string DisplayName)
    : IRequest<Result<CustomerEntity>>;
