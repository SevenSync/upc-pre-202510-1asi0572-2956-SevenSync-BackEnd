using System.Net.Mime;
using MaceTech.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using MaceTech.API.IAM.Interfaces.ACL;
using MaceTech.API.SP.Domain.Model.Commands;
using MaceTech.API.SP.Domain.Model.Queries;
using MaceTech.API.SP.Domain.Services;
using MaceTech.API.SP.Interfaces.REST.Subscription.Resources;
using MaceTech.API.SP.Interfaces.REST.Subscription.Responses;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace MaceTech.API.SP.Interfaces.REST.Subscription;

[ApiController]
[Route("api/v1/[controller]/me")]
[Produces(MediaTypeNames.Application.Json)]
public class SubscriptionsController(
    ISubscriptionQueryService subscriptionQueryService,
    ISubscriptionCommandService subscriptionCommandService,
    IIamContextFacade iamContextFacade
    ): ControllerBase
{
    //  |: Variables
    private readonly SubscriptionService _stripeSubs = new();
    
    //  |: Functions
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetMySubscription()
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
    [HttpDelete]
    public async Task<IActionResult> CancelMySubscription([FromBody] CancelSubscriptionResource resource)
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