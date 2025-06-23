using MaceTech.API.Analytics.Domain.Model.Queries;
using MaceTech.API.Analytics.Domain.Model.ValueObjects;

namespace MaceTech.API.Analytics.Domain.Services.QueriesServices;

public interface IAnalyticsQueryService
{
    Task<IEnumerable<PotComparisonData>> Handle(GetPotComparisonQuery query);
}