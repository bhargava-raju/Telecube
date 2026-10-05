using Customer.Application.Abstractions;
using Customer.Application.Queries.GetCustomerByEntraId;
using Customer.UnitTests.Fakers;
using Moq;
using Telecube.BuildingBlocks.Common;

namespace Customer.UnitTests.Application;

public class GetCustomerByEntraIdQueryHandlerTests
{
    private readonly Mock<ICustomerRepository> _repository = new();
    private readonly GetCustomerByEntraIdQueryHandler _sut;

    public GetCustomerByEntraIdQueryHandlerTests()
    {
        _sut = new GetCustomerByEntraIdQueryHandler(_repository.Object);
    }

    [Fact]
    public async Task Handle_WhenCustomerExists_ReturnsCustomer()
    {
        // Arrange
        var customer = new CustomerFaker().Generate();
        _repository
            .Setup(r => r.GetByEntraObjectIdAsync(customer.EntraObjectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(customer);

        // Act
        var result = await _sut.Handle(new GetCustomerByEntraIdQuery(customer.EntraObjectId), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeSameAs(customer);
    }

    [Fact]
    public async Task Handle_WhenCustomerDoesNotExist_ReturnsNotFound()
    {
        // Act
        var result = await _sut.Handle(new GetCustomerByEntraIdQuery("unknown-oid"), CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.NotFound);
    }
}
