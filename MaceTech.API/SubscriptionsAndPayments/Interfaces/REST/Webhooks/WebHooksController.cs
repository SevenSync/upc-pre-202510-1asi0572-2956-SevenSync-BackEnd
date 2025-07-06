using System.Net.Mime;
using MaceTech.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using MaceTech.API.SubscriptionsAndPayments.Application.External.Sku.Requests;
using MaceTech.API.SubscriptionsAndPayments.Application.External.Sku.Services;
using MaceTech.API.SubscriptionsAndPayments.Domain.Model.Commands;
using MaceTech.API.SubscriptionsAndPayments.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace MaceTech.API.SubscriptionsAndPayments.Interfaces.REST.Webhooks;

[ApiController]
[Route("api/v1/[controller]/")]
[Produces(MediaTypeNames.Application.Json)]
public class WebHooksController(
    ISubscriptionCommandService subscriptionCommandService,
    ISkuAndPriceIdConverter skuAndProductIdConverter
    ): ControllerBase
{
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Handle()
    {
        //  Code for development purposes only. Do not use in production.
        //  this.code set @DevelopmentOnly description "do not use in production"
        
        var json                = await new StreamReader(Request.Body).ReadToEndAsync();
        var sigHeader           = Request.Headers["Stripe-Signature"];
        const string webhook    = "whsec_d5f84a049d46b0af299e9f653403ffa486baa8c0ac0ad43ea709db4b14b7b2d2";

        Stripe.Event evt;
        try
        {
            evt = EventUtility.ConstructEvent(json, sigHeader, webhook);
        }
        catch (StripeException e)
        {
            return BadRequest();
        }
        
        switch (evt.Type)
        {
            case EventTypes.InvoicePaymentSucceeded:
            {
                var invoice     = evt.Data.Object as Invoice;
                var lineItem    = invoice!.Lines.Data.First();
                var sku         = skuAndProductIdConverter.Convert(new ConvertPriceIdToSkuRequest(lineItem.Pricing.PriceDetails.Price));
                var session     = evt.Data.Object as Stripe.Checkout.Session;
                var userId      = Guid.Parse(session!.Metadata["userId"]);
                var command     = new CreateActiveSubscriptionCommand(
                    userId.ToString(), 
                    skuAndProductIdConverter.Convert(new ConvertSkuToSubscriptionPlanType(sku)),
                    session.SubscriptionId);
                
                await subscriptionCommandService.Handle(command);
                
                break;
            }
            case EventTypes.InvoicePaymentFailed:
            {
                Console.WriteLine("Warning, your subscription payment has failed.");
                
                //  We should notify the user about the failed payment. And as "we", I mean "me".
                //  Should I remove these comments?
                //  This code is for development purposes only since the real thing
                //  costs money~
                //  Unlike other controllers which are final and can be developed by anyone within the team.
                
                break;
            }
            case EventTypes.CustomerSubscriptionDeleted:
            {
                Console.WriteLine("Your subscription is over.");
                break;
            }
        }

        return Ok();
    }
}