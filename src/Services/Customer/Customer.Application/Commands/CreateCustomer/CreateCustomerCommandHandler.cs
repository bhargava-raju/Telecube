using Customer.Application.Abstractions;
using MediatR;
using Telecube.BuildingBlocks.Common;
using CustomerEntity = Customer.Domain.Customer;

namespace Customer.Application.Commands.CreateCustomer;

public class CreateCustomerCommandHandler(ICustomerRepository customerRepository)
    : IRequestHandler<CreateCustomerCommand, Result<CustomerEntity>>
{
    public async Task<Result<CustomerEntity>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var existingCustomer = await customerRepository.GetByEntraObjectIdAsync(request.EntraObjectId, cancellationToken);
            if (existingCustomer != null)
            {
                return Result<CustomerEntity>.Failure(new Error("Conflict", "Customer already exists", ErrorType.Conflict));
            }

            var customerResult = CustomerEntity.Create(request.EntraObjectId, request.Email, request.DisplayName);
            if (customerResult.IsFailure)
            {
                return Result<CustomerEntity>.Failure(customerResult.Error);
            }
            await customerRepository.AddAsync(customerResult.Value, cancellationToken);
            return Result<CustomerEntity>.Success(customerResult.Value);
        }
        catch (Exception ex)
        {
            return Result<CustomerEntity>
                .Failure(new Error("Failure", $"CreateCustomerCommandHandler.Handle : {ex.Message}", ErrorType.Failure));
        }
    }
}
