using Automatonymous;
using Festify.Sales.Messages.Payments;
using Festify.Sales.Messages.Purchases;
using Festify.Sales.States;

namespace Festify.Sales
{
    public class SalesStateMachine : MassTransitStateMachine<SalesState>
    {
        public State Started { get; private set; }

        public SalesStateMachine()
        {
            InstanceState(x => x.CurrentState);

            Event(
                () => PurchaseSubmitted,
                x => x.CorrelateById(context => context.Message.purchaseGuid));

            Initially(
                When(PurchaseSubmitted)
                    .Then(x => x.Publish(new ReserveFunds
                    {
                        reservation = new ReservationRepresentation
                        {
                            amount = x.Data.purchase.itemTotal
                        }
                    }))
                    .TransitionTo(Started)
            );
        }

        public Event<PurchaseSubmitted> PurchaseSubmitted { get; private set; }
    }
}