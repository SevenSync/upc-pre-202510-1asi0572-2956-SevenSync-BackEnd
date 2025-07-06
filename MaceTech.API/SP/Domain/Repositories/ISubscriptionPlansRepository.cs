using MaceTech.API.SP.Domain.Model.ValueObjects;

namespace MaceTech.API.SP.Domain.Repositories;

public interface ISubscriptionPlansRepository
{
    public List<SubscriptionPlan> GetPlans(string language);
}