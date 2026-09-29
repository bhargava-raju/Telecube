using Customer.Application.Abstractions;
using MediatR;
using Telecube.BuildingBlocks.Common;
using CustomerEntity = Customer.Domain.Customer;

namespace Customer.Application.Queries.GetCustomerByEntraId;

public class GetCustomerByEntraIdQueryHandler(ICustomerRepository customerRepository)
    : IRequestHandler<GetCustomerByEntraIdQuery, Result<CustomerEntity>>
{
    public async Task<Result<CustomerEntity>> Handle(GetCustomerByEntraIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetByEntraObjectIdAsync(request.EntraObjectId, cancellationToken);
        if (customer == null)
        {
            return Result<CustomerEntity>.Failure(new Error("Failure", "Customer not found", ErrorType.Failure));
        }
        return Result<CustomerEntity>.Success(customer);       
    }
}
