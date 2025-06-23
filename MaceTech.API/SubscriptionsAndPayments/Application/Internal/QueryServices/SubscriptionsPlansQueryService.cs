using MaceTech.API.SubscriptionsAndPayments.Domain.Model.Queries;
using MaceTech.API.SubscriptionsAndPayments.Domain.Model.ValueObjects;
using MaceTech.API.SubscriptionsAndPayments.Domain.Repositories;
using MaceTech.API.SubscriptionsAndPayments.Domain.Services;

namespace MaceTech.API.SubscriptionsAndPayments.Application.Internal.QueryServices;

public class SubscriptionsPlansQueryService(ISubscriptionPlansRepository repository) : ISubscriptionPlansQueryService
{
    public List<SubscriptionPlan> Handle(GetPlansQuery query) => repository.GetPlans(query.Language);
}