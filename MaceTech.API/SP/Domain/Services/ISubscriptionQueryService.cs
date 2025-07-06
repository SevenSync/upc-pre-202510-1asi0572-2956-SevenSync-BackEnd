using MaceTech.API.SP.Domain.Model.Aggregates;
using MaceTech.API.SP.Domain.Model.Queries;

namespace MaceTech.API.SP.Domain.Services;

public interface ISubscriptionQueryService
{
    public Task<Subscription?> Handle(GetSubscriptionStatusQuery query);
}