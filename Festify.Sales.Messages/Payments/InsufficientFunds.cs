using System;

namespace Festify.Sales.Messages.Payments
{
    public class InsufficientFunds
    {
        public Guid purchaseGuid { get; set; }
        public ReservationRepresentation reservation { get; set; }
    }
}
