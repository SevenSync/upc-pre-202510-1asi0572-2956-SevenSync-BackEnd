using MaceTech.API.Analytics.Domain.Model.Queries;
using MaceTech.API.Analytics.Domain.Services.QueriesServices;
using MaceTech.API.Analytics.Interfaces.ACL.DTOs;

namespace MaceTech.API.Analytics.Interfaces.ACL.Services;

public class AlertsContextFacade(IAlertQueryService alertQueryService) : IAlertsContextFacade
{
    public async Task<IEnumerable<AlertDataDto>> FetchAlertsByDeviceIdAndDateRange(string deviceId, DateTime fromDate, DateTime toDate)
    {
        var query = new GetAlertsByDeviceIdAndDateRangeQuery(deviceId, fromDate, toDate);
        var alerts = await alertQueryService.Handle(query);

        return alerts.Select(alert => new AlertDataDto(alert.GeneratedRecommendation.Urgency));
    }
}