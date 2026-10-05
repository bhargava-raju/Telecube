using Telecube.BuildingBlocks.Common;
namespace Customer.Domain;

public class Customer : AggregateRoot
{
    public string EntraObjectId { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string DisplayName { get; private set; } = null!;
    private Customer() { } 
    public static Result<Customer> Create(string entraObjectId, string email, string displayName)
    {
        var result = Guard.AgainstNullOrEmpty(entraObjectId, "EntraObjectId");
        if (result.IsFailure)
            return Result<Customer>.Failure(result.Error);

        result = Guard.AgainstNullOrEmpty(email, "Email");
        if (result.IsFailure)
            return Result<Customer>.Failure(result.Error);

        result = Guard.AgainstNullOrEmpty(displayName, "DisplayName");
        if (result.IsFailure)
            return Result<Customer>.Failure(result.Error);

        Customer customer = new()
        {
            DisplayName = displayName,
            Email = email,
            EntraObjectId = entraObjectId,
            Id = Guid.NewGuid()
        };
        return Result<Customer>.Success(customer);
    }
    public Result UpdateProfile(string email, string displayName)
    {
        var result = Guard.AgainstNullOrEmpty(email, "Email");
        if (result.IsFailure)
            return Result.Failure(result.Error);

        result = Guard.AgainstNullOrEmpty(displayName, "DisplayName");
        if (result.IsFailure)
            return Result.Failure(result.Error);

        Email = email;  
        DisplayName = displayName;

        AddDomainEvent(new CustomerUpdatedDomainEvent(Id, email, displayName));
        return Result.Success();
    }
}