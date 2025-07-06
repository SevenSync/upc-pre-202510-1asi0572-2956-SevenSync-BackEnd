using MaceTech.API.Shared.Domain.Repositories;
using MaceTech.API.SP.Domain.Model.Aggregates;
using MaceTech.API.SP.Domain.Model.Enums;

namespace MaceTech.API.SP.Domain.Repositories;

public interface ISubscriptionRepository : IBaseRepository<Subscription>
{
    //  There can be just one active subscription per user, so we can use the user's UID to find it.
    public Task<Subscription?> FindActiveSubscriptionByUidAsync(string uid);
    
    //  This method is used to find a subscription of a user and a specific type.
    public Task<Subscription?> FindSubscriptionByUserIdAndTypeAsync(string uid, SubscriptionPlanType type);
}