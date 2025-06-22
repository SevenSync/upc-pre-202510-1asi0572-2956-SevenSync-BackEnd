using MaceTech.API.Analytics.Domain.Model.Aggregates;
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

        // Mapeo de la entidad de dominio 'Alert' al 'AlertDataDto'
        return alerts.Select(alert => new AlertDataDto(alert.GeneratedRecommendation.Urgency));
    }
}