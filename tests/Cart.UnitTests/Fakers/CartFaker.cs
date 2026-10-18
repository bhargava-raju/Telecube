using Bogus;
using CartEntity = Cart.Domain.Cart;

namespace Cart.UnitTests.Fakers;

public sealed class CartFaker : Faker<CartEntity>
{
    // TODO: Build each fake cart through the cart's own creation method, then add a few random valid items.
    public CartFaker()
    {
    }
}
