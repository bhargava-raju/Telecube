using Customer.Application.Abstractions;
using Customer.Application.Commands.UpdateCustomerProfile;
using Customer.UnitTests.Fakers;
using Moq;
using Telecube.BuildingBlocks.Common;
using Telecube.Contracts;
using CustomerEntity = Customer.Domain.Customer;

namespace Customer.UnitTests.Application;

public class UpdateCustomerProfileCommandHandlerTests
{
    private const string NewEmail = "new@telecube.test";
    private const string NewDisplayName = "New Name";

    private readonly Mock<ICustomerRepository> _repository = new();
    private readonly Mock<IIntegrationEventPublisher> _publisher = new();
    private readonly UpdateCustomerProfileCommandHandler _sut;

    public UpdateCustomerProfileCommandHandlerTests()
    {
        _sut = new UpdateCustomerProfileCommandHandler(_repository.Object, _publisher.Object);
    }

    [Fact]
    public async Task Handle_WhenCustomerExists_UpdatesAndPublishesCustomerUpdated()
    {
        // Arrange
        var customer = new CustomerFaker().Generate();
        _repository
            .Setup(r => r.GetByEntraObjectIdAsync(customer.EntraObjectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(customer);
        var command = new UpdateCustomerProfileCommand(customer.EntraObjectId, NewEmail, NewDisplayName);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _repository.Verify(
            r => r.UpdateAsync(customer, It.IsAny<CancellationToken>()),
            Times.Once);
        _publisher.Verify(
            p => p.PublishAsync(
                It.Is<CustomerUpdated>(e =>
                    e.CustomerId == customer.Id &&
                    e.Email == NewEmail &&
                    e.DisplayName == NewDisplayName),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenCustomerDoesNotExist_ReturnsNotFoundAndDoesNotPublish()
    {
        // Arrange
        var command = new UpdateCustomerProfileCommand("unknown-oid", NewEmail, NewDisplayName);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.NotFound);
        _repository.Verify(
            r => r.UpdateAsync(It.IsAny<CustomerEntity>(), It.IsAny<CancellationToken>()),
            Times.Never);
        _publisher.Verify(
            p => p.PublishAsync(It.IsAny<CustomerUpdated>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenSaveThrows_ReturnsFailureAndDoesNotPublish()
    {
        // Arrange
        var customer = new CustomerFaker().Generate();
        _repository
            .Setup(r => r.GetByEntraObjectIdAsync(customer.EntraObjectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(customer);
        _repository
            .Setup(r => r.UpdateAsync(customer, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Database unavailable"));
        var command = new UpdateCustomerProfileCommand(customer.EntraObjectId, NewEmail, NewDisplayName);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Failure);
        _publisher.Verify(
            p => p.PublishAsync(It.IsAny<CustomerUpdated>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
