using static VerifyXunit.Verifier;

namespace Telecube.Contracts.UnitTests;

public class CustomerUpdatedTests
{
    [Fact]
    public Task CustomerUpdated_HasExpectedShape()
    {
        var customerUpdated = new CustomerUpdated(
            CustomerId: Guid.Parse("6f1c2a4e-0000-0000-0000-000000000001"),
            Email: "ada@telecube.test",
            DisplayName: "Ada Lovelace");

        return Verify(customerUpdated);
    }
}
