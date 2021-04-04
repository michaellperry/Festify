using System;

namespace Festify.Sales.Messages.Purchases
{
    public class PurchaseRepresentation
    {
        public decimal itemTotal { get; set; }
        public string itemSku { get; set; }
        public int itemQuantity { get; set; }
    }
}
