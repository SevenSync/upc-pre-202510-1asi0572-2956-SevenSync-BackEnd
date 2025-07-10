using MaceTech.API.Watering.Domain.Model.Aggregates;
using MaceTech.API.Watering.Domain.Model.Entity;
using MaceTech.API.Watering.Domain.Model.Queries;
using MaceTech.API.Watering.Domain.Repositories;
using MaceTech.API.Watering.Domain.Services.QueryServices;

namespace MaceTech.API.Watering.Application.Internal.QueryServices;

public class WateringHistoryQueryService(IWateringLogRepository wateringLogRepository) : IWateringHistoryQueryService
{
    public async Task<WateringHistory> Handle(GetWateringHistoryByDeviceIdAndDateRangeQuery query)
    {
        return await wateringLogRepository.FindByDeviceIdAndDateRangeAsync(query.DeviceId, query.From, query.To);
    }
    
    public async Task<WateringHistory> Handle(GetWateringHistoryByDeviceIdQuery query)
    {
        return await wateringLogRepository.FindByDeviceIdAsync(query.DeviceId);
    }
}