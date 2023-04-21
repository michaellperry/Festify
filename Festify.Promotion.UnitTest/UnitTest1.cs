using MassTransit.Testing;

namespace Festify.Promotion.UnitTest;

public class PromotionServiceTests
{
    [Fact]
    public void WhenCustomerPurchasesItem_ThenPurchaseIsPublished()
    {
        InMemoryTestHarness harness = new InMemoryTestHarness();
    }
}