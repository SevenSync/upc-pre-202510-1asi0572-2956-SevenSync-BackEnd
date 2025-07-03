using MaceTech.API.Analytics.Interfaces.ACL.DTOs;
using MaceTech.API.Watering.Interfaces.ACL;

namespace MaceTech.API.Analytics.Interfaces.ACL.Services;

public class WateringLogContextFacade(IWateringContextFacade wateringContextFacade) : IWateringLogContextFacade
{
    public async Task<IEnumerable<WateringLogDataDto>> GetWateringLogHistoryForDevice(string deviceId, DateTime fromDate, DateTime toDate)
    {
        return await wateringContextFacade.FetchWateringLogsByDeviceIdAndDateRange(deviceId, fromDate, toDate);
    }
}