using NetArchTest.Rules;

namespace Telecube.ArchitectureTests;

public class LayerDependencyTests
{
    [Fact]
    public void Domain_ShouldNotDependOnOtherLayersOrFrameworks()
    {
        var result = Types.InAssembly(Assemblies.Domain)
            .ShouldNot()
            .HaveDependencyOnAny(
                "Customer.Application", "Customer.Infrastructure", "Customer.Api",
                "Dapper", "MediatR", "MassTransit")
            .GetResult();

        result.IsSuccessful.Should().BeTrue("these types break the rule: {0}", result.FailingTypeNames());
    }

    [Fact]
    public void Application_ShouldNotDependOnInfrastructureApiOrDataAccess()
    {
        var result = Types.InAssembly(Assemblies.Application)
            .ShouldNot()
            .HaveDependencyOnAny(
                "Customer.Infrastructure", "Customer.Api",
                "Dapper", "Microsoft.Data.SqlClient", "MassTransit")
            .GetResult();

        result.IsSuccessful.Should().BeTrue("these types break the rule: {0}", result.FailingTypeNames());
    }

    [Fact]
    public void Infrastructure_ShouldNotDependOnApi()
    {
        var result = Types.InAssembly(Assemblies.Infrastructure)
            .ShouldNot()
            .HaveDependencyOnAny("Customer.Api")
            .GetResult();

        result.IsSuccessful.Should().BeTrue("these types break the rule: {0}", result.FailingTypeNames());
    }
}
