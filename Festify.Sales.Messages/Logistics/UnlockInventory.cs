using System;

namespace Festify.Sales.Messages.Logistics
{
    public class UnlockInventory
    {
        public Guid purchaseGuid { get; set; }
        public InventoryRepresentation inventory { get; set; }
    }
}
