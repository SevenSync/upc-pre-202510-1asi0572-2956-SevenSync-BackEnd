using MaceTech.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using MaceTech.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using MaceTech.API.SubscriptionsAndPayments.Domain.Model.Aggregates;
using MaceTech.API.SubscriptionsAndPayments.Domain.Model.Enums;
using MaceTech.API.SubscriptionsAndPayments.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MaceTech.API.SubscriptionsAndPayments.Infrastructure.Persistence.EFC.Repositories;

public class SubscriptionRepository(
    AppDbContext context
    ) : BaseRepository<Subscription>(context), ISubscriptionRepository
{
    public async Task<Subscription?> FindActiveSubscriptionByUidAsync(string uid)
    {
        var now = DateTimeOffset.UtcNow;  
        return await Context.Set<Subscription>().Where(s => s.Uid == uid && s.ActiveUntil > now).FirstOrDefaultAsync();
    }

    public async Task<Subscription?> FindSubscriptionByUserIdAndTypeAsync(string uid, SubscriptionPlanType type)
    {   
        return await Context.Set<Subscription>().Where(s => s.Uid == uid && s.Plan == type && s.IsActive())
            .FirstOrDefaultAsync();
    }
}