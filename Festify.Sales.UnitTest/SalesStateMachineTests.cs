using System;
using System.Threading.Tasks;

using Festify.Sales.Messages;
using Festify.Sales.States;

using FluentAssertions;

using MassTransit.Testing;

using Xunit;

namespace Festify.Sales.UnitTest
{
    public class SalesStateMachineTests : IAsyncLifetime
    {
        private readonly SalesStateMachine machine;
        private readonly InMemoryTestHarness harness;
        private readonly StateMachineSagaTestHarness<SalesState, SalesStateMachine> sagaHarness;

        public SalesStateMachineTests()
        {
            machine = new SalesStateMachine();
            harness = new InMemoryTestHarness();
            sagaHarness = harness.StateMachineSaga<SalesState, SalesStateMachine>(machine);
        }

        public async Task InitializeAsync()
        {
            await harness.Start();
        }

        public async Task DisposeAsync()
        {
            await harness.Stop();
        }

        [Fact]
        public async Task WhenPurchaseSubmitted_SaleIsStarted()
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
    }
}
