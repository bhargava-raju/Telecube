using Customer.Application.Abstractions;
using MediatR;
using Telecube.BuildingBlocks.Common;
using Telecube.Contracts;

namespace Customer.Application.Commands.UpdateCustomerProfile;

public class UpdateCustomerProfileCommandHandler(ICustomerRepository customerRepository, IIntegrationEventPublisher integrationEventPublisher)
    : IRequestHandler<UpdateCustomerProfileCommand, Result>
{
    public async Task<Result> Handle(UpdateCustomerProfileCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var existingCustomerResult = await customerRepository.GetByEntraObjectIdAsync(request.EntraObjectId, cancellationToken);
            if (existingCustomerResult == null)
            {
                return Result.Failure(new Error("NotFound", "Customer not found", ErrorType.NotFound));
            }

            var result = existingCustomerResult.UpdateProfile(request.Email, request.DisplayName);
            if (result.IsFailure)
            {
                return Result.Failure(result.Error);
            }

            await customerRepository.UpdateAsync(existingCustomerResult, cancellationToken);
            await integrationEventPublisher.PublishAsync(new CustomerUpdated(existingCustomerResult.Id, request.Email, request.DisplayName), cancellationToken);
            return Result.Success();

        }
        catch (Exception ex)
        {
            return Result.Failure(new Error("Failure", $"Customer retrieval failed: {ex.Message}", ErrorType.Failure));
        }
    }
}
