using Festify.Sales.States;
using MassTransit;

namespace Festify.Sales
{
    public class SalesStateMachine : MassTransitStateMachine<SalesState>
    {
        public State Started { get; private set; }

        public SalesStateMachine()
        {
            InstanceState(x => x.CurrentState);
        }
    }
}