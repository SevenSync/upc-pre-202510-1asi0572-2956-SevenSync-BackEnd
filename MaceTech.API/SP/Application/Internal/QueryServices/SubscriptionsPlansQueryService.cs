using MaceTech.API.SP.Domain.Model.Queries;
using MaceTech.API.SP.Domain.Model.ValueObjects;
using MaceTech.API.SP.Domain.Repositories;
using MaceTech.API.SP.Domain.Services;

namespace MaceTech.API.SP.Application.Internal.QueryServices;

public class SubscriptionsPlansQueryService(ISubscriptionPlansRepository repository) : ISubscriptionPlansQueryService
{
    public List<SubscriptionPlan> Handle(GetPlansQuery query) => repository.GetPlans(query.Language);
}