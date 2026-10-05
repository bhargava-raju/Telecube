using Bogus;
using CustomerEntity = Customer.Domain.Customer;

namespace Customer.UnitTests.Fakers;

public sealed class CustomerFaker : Faker<CustomerEntity>
{
    public CustomerFaker()
    {
        CustomInstantiator(f => CustomerEntity.Create(
            f.Random.Guid().ToString(),
            f.Internet.Email(),
            f.Name.FullName()).Value);
    }
}
