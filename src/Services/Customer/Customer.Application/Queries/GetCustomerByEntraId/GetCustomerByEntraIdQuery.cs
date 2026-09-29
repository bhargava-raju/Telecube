using MediatR;
using Telecube.BuildingBlocks.Common;
using CustomerEntity = Customer.Domain.Customer;

namespace Customer.Application.Queries.GetCustomerByEntraId;

public record GetCustomerByEntraIdQuery(string EntraObjectId) : IRequest<Result<CustomerEntity>>;
