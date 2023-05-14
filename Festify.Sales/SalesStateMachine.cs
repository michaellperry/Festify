using Festify.Sales.Messages.Payments;
using Festify.Sales.Messages.Purchases;
using Festify.Sales.States;
using MassTransit;

namespace Festify.Sales
{
    public class SalesStateMachine : MassTransitStateMachine<SalesState>
    {
        public State Started { get; private set; }

        public Event<PurchaseSubmitted> PurchaseSubmitted { get; private set; }

        public SalesStateMachine()
        {
            InstanceState(x => x.CurrentState);

            Event(() => PurchaseSubmitted, config =>
                config.CorrelateById(context => context.Message.purchaseGuid)
            );

            Initially(
                When(PurchaseSubmitted)
                    .Publish(context => new ReserveFunds
                    {
                        purchaseGuid = context.Message.purchaseGuid,
                        reservation = new ReservationRepresentation
                        {
                            amount = context.Message.purchase.itemTotal
                        }
                    })
                    .TransitionTo(Started)
            );
        }
    }
}