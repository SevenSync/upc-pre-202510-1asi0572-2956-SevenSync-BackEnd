using MaceTech.API.Planning.Domain.Model.Aggregates;
using MaceTech.API.Planning.Domain.Model.Queries;
using MaceTech.API.Planning.Domain.Repositories;
using MaceTech.API.Planning.Domain.Services.QueryServices;

namespace MaceTech.API.Planning.Application.Internal.QueryServices;

public class DevicePlantQueryService(IDevicePlantRepository devicePlantRepository) : IDevicePlantQueryService
{
    public async Task<DevicePlant?> Handle(GetPlantSettingsByDeviceIdQuery query)
    {
        return await devicePlantRepository.FindByDeviceIdAsync(query.DeviceId);
    }
}