using NetArchTest.Rules;

namespace Telecube.ArchitectureTests;

public class ServiceBoundaryTests
{
    private static readonly string[] OtherServices =
        ["Catalog", "Search", "Cart", "Inventory", "Checkout", "Payment", "Order", "Notification"];

    [Fact]
    public void Customer_ShouldNotDependOnOtherServices()
    {
        var result = Types.InAssemblies(
                [Assemblies.Domain, Assemblies.Application, Assemblies.Infrastructure, Assemblies.Api])
            .ShouldNot()
            .HaveDependencyOnAny(OtherServices)
            .GetResult();

        result.IsSuccessful.Should().BeTrue("these types break the rule: {0}", result.FailingTypeNames());
    }

    [Fact]
    public void Contracts_ShouldNotDependOnAnything()
    {
        var result = Types.InAssembly(Assemblies.Contracts)
            .ShouldNot()
            .HaveDependencyOnAny([.. OtherServices, "Customer", "Telecube.BuildingBlocks", "MassTransit"])
            .GetResult();

        result.IsSuccessful.Should().BeTrue("these types break the rule: {0}", result.FailingTypeNames());
    }
}
