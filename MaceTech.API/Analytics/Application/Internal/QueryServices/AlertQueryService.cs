using MaceTech.API.Analytics.Domain.Model.Aggregates;
using MaceTech.API.Analytics.Domain.Model.Queries;
using MaceTech.API.Analytics.Domain.Repositories;
using MaceTech.API.Analytics.Domain.Services.QueriesServices;

namespace MaceTech.API.Analytics.Application.Internal.QueryServices;

public class AlertQueryService(IAlertRepository alertRepository) : IAlertQueryService
{
    public async Task<IEnumerable<Alert>> Handle(GetAlertsByDeviceIdAndDateRangeQuery query)
    {
        return await alertRepository.FindByDeviceIdAndDateRangeAsync(query.DeviceId, query.From, query.To);
    }
    
    public async Task<IEnumerable<Alert>> Handle(GetAlertsByDeviceIdQuery query)
    {
        return await alertRepository.FindByDeviceIdWithFiltersAsync(query.DeviceId, query.FromDate, query.ToDate, query.AlertType);
    }
}