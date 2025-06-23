
namespace MaceTech.API.SubscriptionsAndPayments.Interfaces.REST.Subscription.Responses;

public class SubscriptionStatusResponse
{
    //  @Properties
    public bool IsPremium { set; get; } = false;
    public Domain.Model.Aggregates.Subscription? Subscription { set; get; } = null;
    
    //  @Constructors
    public SubscriptionStatusResponse() { }
    public SubscriptionStatusResponse(bool isPremium, Domain.Model.Aggregates.Subscription? subscription)
    {
        IsPremium = isPremium;
        Subscription = subscription;
    }
}