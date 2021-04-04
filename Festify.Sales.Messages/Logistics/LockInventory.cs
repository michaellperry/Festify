using System;

namespace Festify.Sales.Messages.Logistics
{
    public class LockInventory
    {
        public Guid purchaseGuid { get; set; }
        public InventoryRepresentation inventory { get; set; }
    }
}
