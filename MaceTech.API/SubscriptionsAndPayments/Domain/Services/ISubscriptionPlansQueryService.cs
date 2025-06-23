using MaceTech.API.SubscriptionsAndPayments.Domain.Model.Queries;
using MaceTech.API.SubscriptionsAndPayments.Domain.Model.ValueObjects;

namespace MaceTech.API.SubscriptionsAndPayments.Domain.Services;

public interface ISubscriptionPlansQueryService
{
    public List<SubscriptionPlan> Handle(GetPlansQuery query);
}