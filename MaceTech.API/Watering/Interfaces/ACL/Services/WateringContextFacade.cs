using MaceTech.API.Watering.Domain.Model.Aggregates;
using MaceTech.API.Watering.Domain.Model.Entity;
using MaceTech.API.Watering.Domain.Model.Queries;
using MaceTech.API.Watering.Domain.Services.QueryServices;

namespace MaceTech.API.Watering.Interfaces.ACL.Services;

public class WateringContextFacade(IWateringHistoryQueryService wateringHistoryQueryService) : IWateringContextFacade
{
    public async Task<WateringHistory> FetchWateringLogsByDeviceIdAndDateRange(long deviceId, DateTime? fromDate, DateTime? toDate)
    {
        var query = new GetWateringHistoryByDeviceIdAndDateRangeQuery(deviceId, fromDate, toDate);
        return await wateringHistoryQueryService.Handle(query);
    }
}