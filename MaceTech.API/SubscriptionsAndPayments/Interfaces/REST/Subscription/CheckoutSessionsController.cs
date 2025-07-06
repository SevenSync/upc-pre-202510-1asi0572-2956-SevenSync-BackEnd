using System.Net.Mime;
using MaceTech.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using MaceTech.API.SubscriptionsAndPayments.Application.External.Sku.Requests;
using MaceTech.API.SubscriptionsAndPayments.Application.External.Sku.Services;
using MaceTech.API.SubscriptionsAndPayments.Interfaces.REST.Subscription.Resources;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace MaceTech.API.SubscriptionsAndPayments.Interfaces.REST.Subscription;

[ApiController]
[Route("api/v1/checkout-sessions")]
[Produces(MediaTypeNames.Application.Json)]
public class CheckoutSessionsController(
    ISkuAndPriceIdConverter skuAndPriceIdConverter
    ): ControllerBase
{
    //  |: Functions
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CheckoutSubscriptionResource resource)
    {
        var priceId = skuAndPriceIdConverter.Convert(new ConvertSkuToPriceIdRequest(resource.Sku));
        var sessionService = new Stripe.Checkout.SessionService();
        var options = new Stripe.Checkout.SessionCreateOptions
        {
            Mode = "subscription",
            CustomerEmail = resource.Email,
            LineItems =
            [
                new SessionLineItemOptions
                {
                    Price = priceId,
                    Quantity = 1
                }
            ],
            SuccessUrl = "https://google.com",
            CancelUrl = "https://microsoft.com"
        };

        var session = await sessionService.CreateAsync(options);
        return Ok(new { checkoutUrl = session.Url });
    }
}