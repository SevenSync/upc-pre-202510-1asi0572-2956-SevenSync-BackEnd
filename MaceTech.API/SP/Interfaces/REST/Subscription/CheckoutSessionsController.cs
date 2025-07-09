using System.Net.Mime;
using MaceTech.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using MaceTech.API.SP.Application.External.Sku.Requests;
using MaceTech.API.SP.Application.External.Sku.Services;
using MaceTech.API.SP.Interfaces.REST.Subscription.Resources;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace MaceTech.API.SP.Interfaces.REST.Subscription;

[ApiController]
[Route("api/v1/checkout-sessions")]
[Produces(MediaTypeNames.Application.Json)]
public class CheckoutSessionsController(
    ISkuAndPriceIdConverter skuAndPriceIdConverter
    ): ControllerBase
{
    //  |: Functions
    /// <summary>
    ///     Creates a checkout session for a subscription.
    /// </summary>
    /// <remarks>
    ///     A checkout session is created for a subscription based on the SKU provided in the request.
    ///     A checkout session allows the user to complete the payment process for a selected subscription.
    ///
    ///     SKUs list:
    /// 
    ///     &#149; <b>premium_plan_monthly</b>: The monthly premium plan.
    /// 
    ///     &#149; <b>premium_plan_yearly</b>: The yearly premium plan.
    /// 
    ///
    ///     Overview of all parameters:
    /// 
    ///     &#149; <b>Email</b>: The associated user's email.
    /// 
    ///     &#149; <b>Sku</b>: A valid SKU product.
    /// </remarks>
    /// <response code="200">Returns a <b>checkout url</b>.</response>
    /// <response code="401">Unauthorized. Check the token.</response>
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