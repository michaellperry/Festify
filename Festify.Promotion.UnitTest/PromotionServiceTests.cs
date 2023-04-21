using Festify.Promotion.Messages.Sales;
using Festify.Promotion.Sales;
using FluentAssertions;
using MassTransit;
using MassTransit.Testing;

namespace Festify.Promotion.UnitTest;

public class PromotionServiceTests
{
    [Fact]
    public async Task WhenCustomerPurchasesItem_ThenPurchaseIsPublished()
    {
        InMemoryTestHarness harness = new InMemoryTestHarness();
        await harness.Start();

        // Arrange
        IPublishEndpoint publishEndpoint = harness.Bus;
        var producer = new PromotionService(publishEndpoint);

        // Act
        await producer.PurchaseTicket();

        await harness.InactivityTask;

        // Assert
        harness.Published.Select<OrderPlaced>()
            .Count().Should().Be(1);

        await harness.Stop();
    }
}