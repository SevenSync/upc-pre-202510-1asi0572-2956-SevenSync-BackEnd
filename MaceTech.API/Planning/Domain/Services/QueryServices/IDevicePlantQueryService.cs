using MaceTech.API.Planning.Domain.Model.Aggregates;
using MaceTech.API.Planning.Domain.Model.Queries;

namespace MaceTech.API.Planning.Domain.Services.QueryServices;

public interface IDevicePlantQueryService
{
    Task<DevicePlant?> Handle(GetPlantSettingsByDeviceIdQuery query);
}