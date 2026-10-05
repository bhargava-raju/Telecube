using MediatR;
using NetArchTest.Rules;

namespace Telecube.ArchitectureTests;

public class ConventionTests
{
    [Fact]
    public void RequestHandlers_ShouldEndWithHandler()
    {
        var result = Types.InAssembly(Assemblies.Application)
            .That().ImplementInterface(typeof(IRequestHandler<,>))
            .Should().HaveNameEndingWith("Handler")
            .GetResult();

        result.IsSuccessful.Should().BeTrue("these types break the rule: {0}", result.FailingTypeNames());
    }

    [Fact]
    public void RepositoryInterfaces_ShouldLiveInAbstractions()
    {
        var result = Types.InAssembly(Assemblies.Application)
            .That().AreInterfaces().And().HaveNameEndingWith("Repository")
            .Should().ResideInNamespace("Customer.Application.Abstractions")
            .GetResult();

        result.IsSuccessful.Should().BeTrue("these types break the rule: {0}", result.FailingTypeNames());
    }

    [Fact]
    public void NonApiLayers_ShouldNotUseMvc()
    {
        var result = Types.InAssemblies(
                [Assemblies.Domain, Assemblies.Application, Assemblies.Infrastructure])
            .ShouldNot()
            .HaveDependencyOnAny("Microsoft.AspNetCore.Mvc")
            .GetResult();

        result.IsSuccessful.Should().BeTrue("these types break the rule: {0}", result.FailingTypeNames());
    }
}
