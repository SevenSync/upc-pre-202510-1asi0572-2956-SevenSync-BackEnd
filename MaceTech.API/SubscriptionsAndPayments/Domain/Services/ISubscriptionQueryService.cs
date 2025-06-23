using MaceTech.API.SubscriptionsAndPayments.Domain.Model.Aggregates;
using MaceTech.API.SubscriptionsAndPayments.Domain.Model.Queries;
using MaceTech.API.SubscriptionsAndPayments.Domain.Model.ValueObjects;

namespace MaceTech.API.SubscriptionsAndPayments.Domain.Services;

public interface ISubscriptionQueryService
{
    public Task<Subscription?> Handle(GetSubscriptionStatusQuery query);
}