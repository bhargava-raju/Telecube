using Customer.Domain;
using CustomerEntity = Customer.Domain.Customer;

namespace Customer.UnitTests;

public class CustomerTests
{
    private const string ValidEntraObjectId = "11111111-1111-1111-1111-111111111111";
    private const string ValidEmail = "ada@telecube.test";
    private const string ValidDisplayName = "Ada Lovelace";
    private const string NewEmail = "newemail@telecube.test";
    private const string NewDisplayName = "New Display Name";

    private static CustomerEntity CreateValidCustomer()
    {
        return CustomerEntity.Create(ValidEntraObjectId, ValidEmail, ValidDisplayName).Value;
    }

    [Fact]
    public void Create_WithValidInput_ReturnsSuccessWithGivenValues()
    {
        // Act
        var result = CustomerEntity.Create(ValidEntraObjectId, ValidEmail, ValidDisplayName);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.EntraObjectId.Should().Be(ValidEntraObjectId);
        result.Value.Email.Should().Be(ValidEmail);
        result.Value.DisplayName.Should().Be(ValidDisplayName);
    }

    [Fact]
    public void Create_WithValidInput_AssignsNewId()
    {
        // Act
        var customer = CreateValidCustomer();

        // Assert
        customer.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void Create_WithValidInput_RaisesNoDomainEvents()
    {
        // Act
        var customer = CreateValidCustomer();

        // Assert
        customer.DomainEvents.Should().BeEmpty();
    }

    [Theory]
    [InlineData(null, ValidEmail, ValidDisplayName, "EntraObjectId")]
    [InlineData("", ValidEmail, ValidDisplayName, "EntraObjectId")]
    [InlineData(ValidEntraObjectId, null, ValidDisplayName, "Email")]
    [InlineData(ValidEntraObjectId, "", ValidDisplayName, "Email")]
    [InlineData(ValidEntraObjectId, ValidEmail, null, "DisplayName")]
    [InlineData(ValidEntraObjectId, ValidEmail, "", "DisplayName")]
    public void Create_WhenAnyFieldIsNullOrEmpty_ReturnsFailure(
        string? entraObjectId, string? email, string? displayName, string expectedParamName)
    {
        // Act
        var result = CustomerEntity.Create(entraObjectId!, email!, displayName!);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Message.Should().EndWith($"Parameter name: {expectedParamName}");
    }

    [Fact]
    public void UpdateProfile_WithValidInput_UpdatesEmailAndDisplayName()
    {
        // Arrange
        var customer = CreateValidCustomer();

        // Act
        var result = customer.UpdateProfile(NewEmail, NewDisplayName);

        // Assert
        result.IsSuccess.Should().BeTrue();
        customer.Email.Should().Be(NewEmail);
        customer.DisplayName.Should().Be(NewDisplayName);
    }

    [Fact]
    public void UpdateProfile_WithValidInput_RaisesCustomerUpdatedDomainEvent()
    {
        // Arrange
        var customer = CreateValidCustomer();

        // Act
        customer.UpdateProfile(NewEmail, NewDisplayName);

        // Assert
        var domainEvent = customer.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<CustomerUpdatedDomainEvent>().Subject;
        domainEvent.Id.Should().Be(customer.Id);
        domainEvent.Email.Should().Be(NewEmail);
        domainEvent.DisplayName.Should().Be(NewDisplayName);
    }

    [Fact]
    public async Task UpdateProfile_DomainEventOccurredOn_IsFixedWhenRaised()
    {
        // Arrange
        var customer = CreateValidCustomer();
        var before = DateTimeOffset.UtcNow;

        // Act
        customer.UpdateProfile(NewEmail, NewDisplayName);
        var after = DateTimeOffset.UtcNow;
        var domainEvent = customer.DomainEvents.Single();
        var firstRead = domainEvent.OccurredOn;
        await Task.Delay(20);
        var secondRead = domainEvent.OccurredOn;

        // Assert
        firstRead.Should().BeOnOrAfter(before).And.BeOnOrBefore(after);
        secondRead.Should().Be(firstRead);
    }

    [Theory]
    [InlineData(null, ValidDisplayName, "Email")]
    [InlineData("", ValidDisplayName, "Email")]
    [InlineData(ValidEmail, null, "DisplayName")]
    [InlineData(ValidEmail, "", "DisplayName")]
    public void UpdateProfile_WhenAnyFieldIsNullOrEmpty_ReturnsFailure(
        string? email, string? displayName, string expectedParamName)
    {
        // Arrange
        var customer = CreateValidCustomer();

        // Act
        var result = customer.UpdateProfile(email!, displayName!);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Message.Should().EndWith($"Parameter name: {expectedParamName}");
    }

    [Fact]
    public void UpdateProfile_WhenInputIsInvalid_LeavesProfileUnchanged()
    {
        // Arrange
        var customer = CreateValidCustomer();

        // Act
        var result = customer.UpdateProfile(NewEmail, "");

        // Assert
        result.IsFailure.Should().BeTrue();
        customer.Email.Should().Be(ValidEmail);
        customer.DisplayName.Should().Be(ValidDisplayName);
        customer.DomainEvents.Should().BeEmpty();
    }
}
