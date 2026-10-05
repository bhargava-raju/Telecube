using Customer.Application.Abstractions;
using Customer.Application.Commands.CreateCustomer;
using Customer.UnitTests.Fakers;
using Moq;
using Telecube.BuildingBlocks.Common;
using CustomerEntity = Customer.Domain.Customer;

namespace Customer.UnitTests.Application;

public class CreateCustomerCommandHandlerTests
{
    private readonly Mock<ICustomerRepository> _repository = new();
    private readonly CreateCustomerCommandHandler _sut;
    private readonly CreateCustomerCommand _command = new("entra-oid-123", "john@telecube.test", "John Smith");

    public CreateCustomerCommandHandlerTests()
    {
        _sut = new CreateCustomerCommandHandler(_repository.Object);
    }

    [Fact]
    public async Task Handle_WhenCustomerDoesNotExist_AddsCustomerAndReturnsSuccess()
    {
        // Act
        var result = await _sut.Handle(_command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.EntraObjectId.Should().Be(_command.EntraObjectId);
        result.Value.Email.Should().Be(_command.Email);
        result.Value.DisplayName.Should().Be(_command.DisplayName);

        _repository.Verify(
            r => r.AddAsync(result.Value, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenCustomerAlreadyExists_ReturnsConflictAndDoesNotAdd()
    {
        // Arrange
        _repository
            .Setup(r => r.GetByEntraObjectIdAsync(_command.EntraObjectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CustomerFaker().Generate());

        // Act
        var result = await _sut.Handle(_command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Conflict);
        result.Error.Message.Should().Be("Customer already exists");

        _repository.Verify(
            r => r.AddAsync(It.IsAny<CustomerEntity>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenInputIsInvalid_ReturnsFailure()
    {
        // Arrange
        var command = _command with { Email = "" };

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Validation);

        _repository.Verify(
            r => r.AddAsync(It.IsAny<CustomerEntity>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrows_ReturnsFailure()
    {
        // Arrange
        _repository
            .Setup(r => r.AddAsync(It.IsAny<CustomerEntity>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Database unavailable"));

        // Act
        var result = await _sut.Handle(_command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Failure);
    }
}
