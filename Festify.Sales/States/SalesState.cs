using Festify.Sales.Messages.Logistics;
using MassTransit;

namespace Festify.Sales.States
{
    public class SalesState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; } = "Initial";

        public InventoryRepresentation Inventory { get; set; } = new();
    }
}