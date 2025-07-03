using MaceTech.API.Planning.Domain.Model.Aggregates;
using MaceTech.API.Shared.Domain.Repositories;

namespace MaceTech.API.Planning.Domain.Repositories;

public interface IDevicePlantRepository : IBaseRepository<DevicePlant>
{
    Task<DevicePlant?> FindByDeviceIdAsync(string deviceId);
}