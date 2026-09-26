namespace Telecube.BuildingBlocks.Messaging;

public static class OutboxExtensions
{
    // TODO: wire MassTransit's EF Core outbox (UseEntityFrameworkOutbox) for services whose
    // DbContext needs to atomically persist state + queue the integration event in one transaction
    // (Inventory, Checkout, Payment, Order — see the design doc's reliability section).
}
