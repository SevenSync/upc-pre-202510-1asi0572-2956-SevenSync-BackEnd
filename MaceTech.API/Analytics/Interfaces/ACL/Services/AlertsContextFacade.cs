using MaceTech.API.Analytics.Domain.Model.Aggregates;
using MaceTech.API.Analytics.Domain.Model.Queries;
using MaceTech.API.Analytics.Domain.Services.QueriesServices;

namespace MaceTech.API.Analytics.Interfaces.ACL.Services;

public class AlertsContextFacade(IAlertQueryService alertQueryService) : IAlertsContextFacade
{
    public async Task<IEnumerable<Alert>> FetchAlertsByDeviceIdAndDateRange(string deviceId, DateTime fromDate, DateTime toDate)
    {
        var query = new GetAlertsByDeviceIdAndDateRangeQuery(deviceId, fromDate, toDate);
        // Cuando el servicio real exista, simplemente pasará la llamada.
        return await alertQueryService.Handle(query);
    }
}