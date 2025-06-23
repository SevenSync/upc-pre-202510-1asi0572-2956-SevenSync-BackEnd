using MaceTech.API.Watering.Domain.Model.Aggregates;
using MaceTech.API.Watering.Domain.Model.Queries;
using MaceTech.API.Watering.Domain.Repositories;
using MaceTech.API.Watering.Domain.Services.QueryServices;

namespace MaceTech.API.Watering.Application.Internal.QueryServices;

public class WateringLogQueryService(IWateringLogRepository wateringLogRepository) : IWateringLogQueryService
{
    public async Task<IEnumerable<WateringLog>> Handle(GetWateringHistoryByDeviceIdQuery query)
    {
        return await wateringLogRepository.FindByDeviceIdAndDateRangeAsync(query.DeviceId, query.From, query.To);
    }
}