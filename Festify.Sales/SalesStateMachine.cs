using System;
using Automatonymous;
using Festify.Sales.Messages.Logistics;
using Festify.Sales.Messages.Payments;
using Festify.Sales.Messages.Purchases;
using Festify.Sales.States;

namespace Festify.Sales
{
    public class SalesStateMachine : MassTransitStateMachine<SalesState>
    {
        public Event<PurchaseSubmitted> PurchaseSubmitted { get; private set; }
        public Event<FundsReserved> FundsReserved { get; private set; }
        public Event<InsufficientFunds> InsufficientFunds { get; private set; }
        public Event<InventoryLocked> InventoryLocked { get; private set; }

        public State Started { get; private set; }
        public State Funded { get; private set; }
        public State Locked { get; private set; }
        public State Failed { get; private set; }

        public SalesStateMachine()
        {
            InstanceState(x => x.CurrentState);

            Event(
                () => PurchaseSubmitted,
                x => x.CorrelateById(context => context.Message.purchaseGuid));
            Event(
                () => FundsReserved,
                x => x.CorrelateById(context => context.Message.purchaseGuid));
            Event(
                () => InventoryLocked,
                x => x.CorrelateById(context => context.Message.purchaseGuid));
            Event(
                () => InsufficientFunds,
                x => x.CorrelateById(context => context.Message.purchaseGuid));

            Initially(
                When(PurchaseSubmitted)
                    .Then(x => x.Publish(new ReserveFunds
                    {
                        purchaseGuid = x.Data.purchaseGuid,
                        reservation = new ReservationRepresentation
                        {
                            amount = x.Data.purchase.itemTotal
                        }
                    }))
                    .Then(x => x.Publish(new LockInventory
                    {
                        purchaseGuid = x.Data.purchaseGuid,
                        inventory = new InventoryRepresentation
                        {
                            sku = x.Data.purchase.itemSku,
                            quantity = x.Data.purchase.itemQuantity
                        }
                    }))
                    .TransitionTo(Started)
            );

            During(Started,
                When(FundsReserved)
                    .TransitionTo(Funded)
            );

            During(Started,
                When(InventoryLocked)
                    .Then(x => x.Instance.Inventory = x.Data.inventory)
                    .TransitionTo(Locked)
            );

            During(Locked,
                When(InsufficientFunds)
                    .Then(x => x.Publish(new UnlockInventory
                    {
                        purchaseGuid = x.Data.purchaseGuid,
                        inventory = x.Instance.Inventory
                    }))
                    .TransitionTo(Failed)
            );
        }
    }
}