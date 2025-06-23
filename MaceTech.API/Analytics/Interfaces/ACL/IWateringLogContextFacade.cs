using MaceTech.API.Analytics.Interfaces.ACL.DTOs;

namespace MaceTech.API.Analytics.Interfaces.ACL;

public interface IWateringLogContextFacade
{
    Task<IEnumerable<WateringLogDataDto>> GetWateringLogHistoryForDevice(string deviceId, DateTime fromDate, DateTime toDate);
}