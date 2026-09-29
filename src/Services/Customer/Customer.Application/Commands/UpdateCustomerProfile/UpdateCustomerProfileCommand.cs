using MediatR;
using Telecube.BuildingBlocks.Common;

namespace Customer.Application.Commands.UpdateCustomerProfile;

public record UpdateCustomerProfileCommand(string EntraObjectId, string Email, string DisplayName)
    : IRequest<Result>;
