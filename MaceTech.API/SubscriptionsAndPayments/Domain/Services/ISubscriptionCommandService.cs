using MaceTech.API.SubscriptionsAndPayments.Domain.Model.Aggregates;
using MaceTech.API.SubscriptionsAndPayments.Domain.Model.Commands;

namespace MaceTech.API.SubscriptionsAndPayments.Domain.Services;

public interface ISubscriptionCommandService
{
    public Task<Subscription?> Handle(CreateActiveSubscriptionCommand command);
    public Task<bool> Handle(CancelSubscriptionCommand command);
}