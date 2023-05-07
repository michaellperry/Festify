using System;

namespace Festify.Sales.Messages.Payments
{
    public class ReserveFunds
    {
        public Guid purchaseGuid { get; set; }
        public ReservationRepresentation reservation { get; set; } = new();
    }
}