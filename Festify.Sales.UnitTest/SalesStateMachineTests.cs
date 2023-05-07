using Festify.Sales.Messages.Logistics;
using Festify.Sales.Messages.Payments;
using Festify.Sales.Messages.Purchases;
using Festify.Sales.States;
using FluentAssertions;
using MassTransit.Testing;

namespace Festify.Sales.UnitTest
{
    public class SalesStateMachineTests : IAsyncLifetime
    {
        private readonly SalesStateMachine machine;
        private readonly InMemoryTestHarness harness;
        private readonly ISagaStateMachineTestHarness<SalesStateMachine, SalesState> sagaHarness;

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

        private async Task<Guid> WhenPurchaseSubmitted(
            decimal itemTotal = 10.00m,
            string itemSku = "DefaultItem",
            int itemQuantity = 1)
        {
            var purchaseGuid = Guid.NewGuid();
            await harness.Bus.Publish(new PurchaseSubmitted
            {
                purchaseGuid = purchaseGuid,
                purchase = new PurchaseRepresentation
                {
                    itemTotal = itemTotal,
                    itemSku = itemSku,
                    itemQuantity = itemQuantity
                }
            });
            return purchaseGuid;
        }

        private async Task WhenFundsReserved()
        {
            var commands = harness.Published.Select<ReserveFunds>().ToList();
            commands.Should().NotBeEmpty();
            foreach (var command in commands)
            {
                ReserveFunds message = command.Context.Message;
                await harness.Bus.Publish(new FundsReserved
                {
                    purchaseGuid = message.purchaseGuid,
                    reservation = message.reservation
                });
                await Task.Delay(10);
            }

            harness.Consumed.Select<FundsReserved>().Should().NotBeEmpty();
        }

        private async Task WhenInsufficientFunds()
        {
            var commands = harness.Published.Select<ReserveFunds>().ToList();
            commands.Should().NotBeEmpty();
            foreach (var command in commands)
            {
                ReserveFunds message = command.Context.Message;
                await harness.Bus.Publish(new InsufficientFunds
                {
                    purchaseGuid = message.purchaseGuid,
                    reservation = message.reservation
                });
                await Task.Delay(10);
            }

            harness.Consumed.Select<InsufficientFunds>().Should().NotBeEmpty();
        }

        private async Task WhenInventoryLocked()
        {
            var commands = harness.Published.Select<LockInventory>().ToList();
            commands.Should().NotBeEmpty();
            foreach (var command in commands)
            {
                LockInventory message = command.Context.Message;
                await harness.Bus.Publish(new InventoryLocked
                {
                    purchaseGuid = message.purchaseGuid,
                    inventory = message.inventory
                });
                await Task.Delay(10);
            }

            harness.Consumed.Select<InventoryLocked>().Should().NotBeEmpty();
        }
    }
}
