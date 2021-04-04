using System;
using Automatonymous;
using Festify.Sales.Messages.Payments;
using Festify.Sales.Messages.Purchases;
using Festify.Sales.States;

namespace Festify.Sales
{
    public class SalesStateMachine : MassTransitStateMachine<SalesState>
    {
        public State Started { get; private set; }
        public State Funded { get; private set; }

        public SalesStateMachine()
        {
            InstanceState(x => x.CurrentState);

            Event(
                () => PurchaseSubmitted,
                x => x.CorrelateById(context => context.Message.purchaseGuid));
            Event(
                () => FundsReserved,
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
                    .TransitionTo(Started)
            );

            During(Started,
                When(FundsReserved)
                    .TransitionTo(Funded)
            );
        }

        public Event<PurchaseSubmitted> PurchaseSubmitted { get; private set; }
        public Event<FundsReserved> FundsReserved { get; private set; }
    }
}