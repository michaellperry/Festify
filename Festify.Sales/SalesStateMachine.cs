using Automatonymous;
using Festify.Sales.Messages;
using Festify.Sales.States;

namespace Festify.Sales
{
    class SalesStateMachine : MassTransitStateMachine<SalesState>
    {
        public State Started { get; private set; }

        public SalesStateMachine()
        {
            InstanceState(x => x.CurrentState);

            Event(
                () => PurchaseSubmitted,
                x => x.CorrelateById(context => context.Message.PurchaseGuid));

            Initially(
                When(PurchaseSubmitted)
                    .TransitionTo(Started)
            );
        }

        public Event<PurchaseSubmitted> PurchaseSubmitted { get; private set; }
    }
}