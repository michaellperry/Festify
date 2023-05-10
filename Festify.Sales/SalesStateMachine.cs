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

            Event(() => PurchaseSubmitted);

            Initially(
                When(PurchaseSubmitted)
                    .TransitionTo(Started)
            );
        }
    }
}