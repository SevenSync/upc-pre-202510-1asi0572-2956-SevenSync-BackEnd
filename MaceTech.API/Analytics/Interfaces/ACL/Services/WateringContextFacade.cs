namespace MaceTech.API.Analytics.Interfaces.ACL.Services;

public class WateringContextFacade(IWateringLogQueryService wateringLogQueryService) : IWateringContextFacade
{
    public async Task<IEnumerable<WateringLog>> FetchWateringLogsByDeviceIdAndDateRange(string deviceId, DateTime fromDate, DateTime toDate)
    {
        var query = new GetWateringHistoryByDeviceIdQuery(deviceId, fromDate, toDate);
        return await wateringLogQueryService.Handle(query);
    }
}