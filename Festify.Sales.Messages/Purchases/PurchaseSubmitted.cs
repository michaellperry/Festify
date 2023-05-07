using System;

namespace Festify.Sales.Messages.Purchases
{
    public class PurchaseSubmitted
    {
        public Guid purchaseGuid { get; set; }
        public PurchaseRepresentation purchase { get; set; } = new();
    }
}
