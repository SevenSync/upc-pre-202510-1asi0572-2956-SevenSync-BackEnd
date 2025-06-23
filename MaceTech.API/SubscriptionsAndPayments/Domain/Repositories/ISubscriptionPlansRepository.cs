using MaceTech.API.SubscriptionsAndPayments.Domain.Model.ValueObjects;

namespace MaceTech.API.SubscriptionsAndPayments.Domain.Repositories;

public interface ISubscriptionPlansRepository
{
    public List<SubscriptionPlan> GetPlans(string language);
}