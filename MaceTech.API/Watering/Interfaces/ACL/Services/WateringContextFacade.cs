using MaceTech.API.Watering.Domain.Model.Aggregates;
using MaceTech.API.Watering.Domain.Model.Queries;
using MaceTech.API.Watering.Domain.Services.QueryServices;

namespace MaceTech.API.Watering.Interfaces.ACL.Services;

public class WateringContextFacade(IWateringLogQueryService wateringLogQueryService) : IWateringContextFacade
{
    public async Task<IEnumerable<WateringLog>> FetchWateringLogsByDeviceIdAndDateRange(string deviceId, DateTime? fromDate, DateTime? toDate)
    {
        var query = new GetWateringHistoryByDeviceIdQuery(deviceId, fromDate, toDate);
        return await wateringLogQueryService.Handle(query);
    }
}