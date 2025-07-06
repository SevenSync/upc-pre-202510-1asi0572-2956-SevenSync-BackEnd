using MaceTech.API.SP.Domain.Model.Queries;
using MaceTech.API.SP.Domain.Model.ValueObjects;

namespace MaceTech.API.SP.Domain.Services;

public interface ISubscriptionPlansQueryService
{
    public List<SubscriptionPlan> Handle(GetPlansQuery query);
}