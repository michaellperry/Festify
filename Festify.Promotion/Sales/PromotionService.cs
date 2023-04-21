using Festify.Promotion.Messages.Sales;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Festify.Promotion.Sales;

public class PromotionService
{
    private IPublishEndpoint publishEndpoint;

    public PromotionService(IPublishEndpoint publishEndpoint)
    {
        this.publishEndpoint = publishEndpoint;
    }

    public async Task PurchaseTicket()
    {
        await publishEndpoint.Publish(new OrderPlaced());
    }
}
