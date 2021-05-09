using Festify.Sales.Messages.Purchases;
using Festify.Sales.States;
using FluentAssertions;
using MassTransit;
using MassTransit.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Festify.Sales.UnitTest
{
    public class SalesIntegrationTest
    {
        [Fact]
        public async Task WhenCustomerPurchasesItem_ThenCustomerIsNotified()
        {
            InMemoryTestHarness harness = new InMemoryTestHarness();
            await harness.Start();

            IPublishEndpoint publishEndpoint = harness.Bus;
            var producer = new PromotionService(publishEndpoint);

            producer.PurchaseTicket();

            await harness.Stop();

            harness.Published.Select<PurchaseSubmitted>().Count().Should().Be(1);
        }
    }
}
