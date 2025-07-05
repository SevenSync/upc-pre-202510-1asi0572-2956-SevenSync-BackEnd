using MaceTech.API.Planning.Domain.Model.Aggregates;
using MaceTech.API.Planning.Domain.Repositories;
using MaceTech.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using MaceTech.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MaceTech.API.Planning.Infrastructure.Persistence.EFC.Repositories;

public class DevicePlantRepository(AppDbContext context) : BaseRepository<DevicePlant>(context), IDevicePlantRepository
{
    public async Task<DevicePlant?> FindByDeviceIdAsync(string deviceId)
    {
        return await Context.Set<DevicePlant>()
            .Include(dp => dp.Plant)
            .FirstOrDefaultAsync(dp => dp.DeviceId == deviceId);
    }
}