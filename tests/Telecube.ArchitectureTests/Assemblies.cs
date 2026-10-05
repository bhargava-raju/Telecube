using System.Reflection;
using Customer.Api.Controllers;
using Customer.Application.Abstractions;
using Customer.Infrastructure;
using Telecube.Contracts;

namespace Telecube.ArchitectureTests;

internal static class Assemblies
{
    public static readonly Assembly Domain = typeof(Customer.Domain.Customer).Assembly;

    public static readonly Assembly Application = typeof(ICustomerRepository).Assembly;

    public static readonly Assembly Infrastructure = typeof(InfrastructureExtensions).Assembly;

    public static readonly Assembly Api = typeof(CustomersController).Assembly;

    public static readonly Assembly Contracts = typeof(IIntegrationEvent).Assembly;
}
