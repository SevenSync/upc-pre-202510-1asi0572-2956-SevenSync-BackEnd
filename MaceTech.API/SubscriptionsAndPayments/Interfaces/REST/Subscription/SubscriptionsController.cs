using System.Net.Mime;
using MaceTech.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using MaceTech.API.IAM.Interfaces.ACL;
using MaceTech.API.SubscriptionsAndPayments.Application.External.Sku.Requests;
using MaceTech.API.SubscriptionsAndPayments.Application.External.Sku.Services;
using MaceTech.API.SubscriptionsAndPayments.Domain.Model.Commands;
using MaceTech.API.SubscriptionsAndPayments.Domain.Model.Queries;
using MaceTech.API.SubscriptionsAndPayments.Domain.Services;
using MaceTech.API.SubscriptionsAndPayments.Interfaces.REST.Subscription.Resources;
using MaceTech.API.SubscriptionsAndPayments.Interfaces.REST.Subscription.Responses;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;

namespace MaceTech.API.SubscriptionsAndPayments.Interfaces.REST.Subscription;

[ApiController]
[Route("api/v1/[controller]/")]
[Produces(MediaTypeNames.Application.Json)]
public class SubscriptionsController(
    ISubscriptionPlansQueryService plansQueryService,
    ISkuAndPriceIdConverter skuAndPriceIdConverter,
    ISubscriptionQueryService subscriptionQueryService,
    ISubscriptionCommandService subscriptionCommandService,
    IIamContextFacade iamContextFacade
    ): ControllerBase
{
    //  |: Variables
    private readonly SubscriptionService _stripeSubs = new();
    
    //  |: Functions
    [Authorize]
    [HttpPost("checkout")]
    public async Task<IActionResult> CreateSubscription([FromBody] CheckoutSubscriptionResource resource)
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
    
    [Authorize]
    [HttpGet("status")]
    public async Task<IActionResult> GetSubscriptionStatus()
    {
        var uid = iamContextFacade.GetUserUidFromContext(this.HttpContext);
        
        var query = new GetSubscriptionStatusQuery(uid);
        var status = await subscriptionQueryService.Handle(query);
        var response = new SubscriptionStatusResponse(false, status);
        if (status != null)
        {
            response.IsPremium = true;
        }
        
        return Ok(response);
    }
    
    [Authorize]
    [HttpPost("cancel")]
    public async Task<IActionResult> CancelSubscription([FromBody] CancelSubscriptionResource resource)
    {
        var localSub = await subscriptionQueryService.Handle(new GetSubscriptionStatusQuery(resource.Uid));
        if (localSub is null)
        {
            return NotFound("No active subscription found for this user.");
        }

        await subscriptionCommandService.Handle(new CancelSubscriptionCommand(localSub.Uid));
        await _stripeSubs.CancelAsync(localSub.SubscriptionId);

        return Ok(new SubscriptionCancelledResponse(Cancelled: true));
    }
}