using MaceTech.API.SP.Domain.Model.Aggregates;
using MaceTech.API.SP.Domain.Model.Queries;
using MaceTech.API.SP.Domain.Repositories;
using MaceTech.API.SP.Domain.Services;

namespace MaceTech.API.SP.Application.Internal.QueryServices;

public class SubscriptionQueryService(ISubscriptionRepository repository) : ISubscriptionQueryService
{
    public async Task<Subscription?> Handle(GetSubscriptionStatusQuery query)
    {
        return await repository.FindActiveSubscriptionByUidAsync(query.Uid);
    }
}