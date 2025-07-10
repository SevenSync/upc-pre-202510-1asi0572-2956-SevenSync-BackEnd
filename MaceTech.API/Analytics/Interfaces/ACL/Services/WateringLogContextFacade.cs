using MaceTech.API.Analytics.Interfaces.ACL.DTOs;
using MaceTech.API.Watering.Domain.Model.Aggregates;
using MaceTech.API.Watering.Interfaces.ACL;

namespace MaceTech.API.Analytics.Interfaces.ACL.Services;

public class WateringLogContextFacade(
    IWateringContextFacade wateringContextFacade
    ) : IWateringLogContextFacade
{
    public async Task<IEnumerable<WateringLogDataDto>> GetWateringLogHistoryForDevice(
        long deviceId, DateTime fromDate, DateTime toDate
        )
    {
        var result = await wateringContextFacade.FetchWateringLogsByDeviceIdAndDateRange(deviceId, fromDate, toDate);
        var wateringLogs = (result?.Historial ?? Enumerable.Empty<WateringLog>()).Select(log => new WateringLogDataDto(log.WaterVolumeMl)).ToList();
        return wateringLogs;
    }
}