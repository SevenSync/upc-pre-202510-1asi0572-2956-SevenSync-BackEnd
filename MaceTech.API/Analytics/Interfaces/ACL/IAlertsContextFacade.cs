using MaceTech.API.Analytics.Interfaces.ACL.DTOs;

namespace MaceTech.API.Analytics.Interfaces.ACL;

public interface IAlertsContextFacade
{
    Task<IEnumerable<AlertDataDto>> FetchAlertsByDeviceIdAndDateRange(string deviceId, DateTime fromDate, DateTime toDate);
}