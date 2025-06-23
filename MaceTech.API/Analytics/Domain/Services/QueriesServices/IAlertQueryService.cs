using MaceTech.API.Analytics.Domain.Model.Aggregates;
using MaceTech.API.Analytics.Domain.Model.Queries;

namespace MaceTech.API.Analytics.Domain.Services.QueriesServices;

public interface IAlertQueryService
{
    Task<IEnumerable<Alert>> Handle(GetAlertsByDeviceIdAndDateRangeQuery query);
}