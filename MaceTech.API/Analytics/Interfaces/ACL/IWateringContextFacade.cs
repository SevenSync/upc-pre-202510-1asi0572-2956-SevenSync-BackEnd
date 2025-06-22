namespace MaceTech.API.Analytics.Interfaces.ACL;

public interface IWateringContextFacade
{
    Task<IEnumerable<WateringLog>> FetchWateringLogsByDeviceIdAndDateRange(string deviceId, DateTime fromDate, DateTime toDate);
}