using System;
using Automatonymous;
using MassTransit;

namespace Festify.Sales.States
{
    public class SalesState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }
    }
}