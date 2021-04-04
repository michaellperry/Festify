using System;
using System.Threading.Tasks;
using Festify.Sales.Messages.Payments;
using Festify.Sales.Messages.Purchases;
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
            var purchaseGuid = await WhenPurchaseSubmitted();

            harness.Consumed.Select<PurchaseSubmitted>().Should().HaveCount(1);
            sagaHarness.Consumed.Select<PurchaseSubmitted>().Should().HaveCount(1);
            sagaHarness.Created.ContainsInState(purchaseGuid, machine, machine.Started)
                .Should().NotBeNull();
        }

        [Fact]
        public async Task WhenPurchaseSubmitted_ReserveFundsIsRequested()
        {
            decimal itemTotal = 3.14m;
            var purchaseGuid = await WhenPurchaseSubmitted(itemTotal: itemTotal);

            harness.Published.Select<ReserveFunds>().Should().HaveCount(1)
                .And.SatisfyRespectively(x =>
                {
                    var reserveFunds = x.Context.Message;
                    reserveFunds.purchaseGuid.Should().Be(purchaseGuid);
                    reserveFunds.reservation.amount.Should().Be(itemTotal);
                });
        }

        [Fact]
        public async Task WhenFundsAreReserved_SaleIsFunded()
        {
            var purchaseGuid = await WhenPurchaseSubmitted();
            await WhenFundsReserved();

            sagaHarness.Created.ContainsInState(purchaseGuid, machine, machine.Funded)
                .Should().NotBeNull();
        }

        private async Task<Guid> WhenPurchaseSubmitted(
            decimal itemTotal = 10.00m)
        {
            var purchaseGuid = Guid.NewGuid();
            await harness.Bus.Publish(new PurchaseSubmitted
            {
                purchaseGuid = purchaseGuid,
                purchase = new PurchaseRepresentation
                {
                    itemTotal = itemTotal
                }
            });
            return purchaseGuid;
        }

        private async Task WhenFundsReserved()
        {
            var commands = harness.Published.Select<ReserveFunds>();
            foreach (var command in commands)
            {
                ReserveFunds message = command.Context.Message;
                await harness.Bus.Publish(new FundsReserved
                {
                    purchaseGuid = message.purchaseGuid,
                    reservation = message.reservation
                });
            }
        }
    }
}
