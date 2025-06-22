using MaceTech.API.Analytics.Domain.Model.Aggregates;

namespace MaceTech.API.Analytics.Interfaces.ACL;

public interface IAlertsContextFacade
{
    Task<IEnumerable<Alert>> FetchAlertsByDeviceIdAndDateRange(string deviceId, DateTime fromDate, DateTime toDate);
}