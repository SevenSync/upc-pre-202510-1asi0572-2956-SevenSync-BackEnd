using MaceTech.API.SubscriptionsAndPayments.Domain.Model.Aggregates;
using MaceTech.API.SubscriptionsAndPayments.Domain.Model.Queries;
using MaceTech.API.SubscriptionsAndPayments.Domain.Repositories;
using MaceTech.API.SubscriptionsAndPayments.Domain.Services;

namespace MaceTech.API.SubscriptionsAndPayments.Application.Internal.QueryServices;

public class SubscriptionQueryService(ISubscriptionRepository repository) : ISubscriptionQueryService
{
    public async Task<Subscription?> Handle(GetSubscriptionStatusQuery query)
    {
        return await repository.FindActiveSubscriptionByUidAsync(query.Uid);
    }
}