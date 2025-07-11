using MaceTech.API.SubscriptionsAndPayments.Domain.Model.Queries;

namespace MaceTech.API.SubscriptionsAndPayments.Interfaces.REST.Subscription.Transform;

public static class GetPlansQueryFromResourceAssembler
{
    public static GetPlansQuery ToQueryFromResource(string language)
    {
        return new GetPlansQuery(language);
    }
}