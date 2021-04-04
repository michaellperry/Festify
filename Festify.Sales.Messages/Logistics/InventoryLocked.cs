using System;

namespace Festify.Sales.Messages.Logistics
{
    public class InventoryLocked
    {
        public Guid purchaseGuid { get; set; }
        public InventoryRepresentation inventory { get; set; }
    }
}
