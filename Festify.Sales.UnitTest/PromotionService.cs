using Festify.Sales.Messages.Purchases;
using MassTransit;
using System;

namespace Festify.Sales.UnitTest
{
    internal class PromotionService
    {
        private IPublishEndpoint publishEndpoint;

        public PromotionService(IPublishEndpoint publishEndpoint)
        {
            this.publishEndpoint = publishEndpoint;
        }

        internal void PurchaseTicket()
        {
            publishEndpoint.Publish(new PurchaseSubmitted
            {
                purchaseGuid = Guid.NewGuid(),
                purchase = new PurchaseRepresentation
                {
                    itemQuantity = 3,
                    itemSku = "Thing",
                    itemTotal = 84.56m
                }
            });
        }
    }
}