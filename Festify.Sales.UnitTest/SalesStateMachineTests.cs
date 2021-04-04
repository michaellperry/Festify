using System;
using System.Threading.Tasks;
using Festify.Sales.Messages;
using Festify.Sales.States;
using FluentAssertions;
using MassTransit.Testing;
using Xunit;

namespace Festify.Sales.UnitTest
{
    public class SalesStateMachineTests
    {
        [Fact]
        public async Task WhenPurchaseSubmitted_SaleIsStarted()
        {
            var machine = new SalesStateMachine();
            var harness = new InMemoryTestHarness();
            var sagaHarness = harness.StateMachineSaga<SalesState, SalesStateMachine>(machine);

            await harness.Start();
            try
            {
                var purchaseGuid = Guid.NewGuid();
                await harness.Bus.Publish(new PurchaseSubmitted
                {
                    PurchaseGuid = purchaseGuid
                });

                harness.Consumed.Select<PurchaseSubmitted>().Should().HaveCount(1);
                sagaHarness.Consumed.Select<PurchaseSubmitted>().Should().HaveCount(1);
                sagaHarness.Created.ContainsInState(purchaseGuid, machine, machine.Started)
                    .Should().NotBeNull();
            }
            finally
            {
                await harness.Stop();
            }
        }
    }
}
