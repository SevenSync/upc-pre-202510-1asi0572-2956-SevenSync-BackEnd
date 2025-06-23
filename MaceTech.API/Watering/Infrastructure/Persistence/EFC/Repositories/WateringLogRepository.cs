using MaceTech.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using MaceTech.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using MaceTech.API.Watering.Domain.Model.Aggregates.MaceTech.API.Watering.Domain.Model.Aggregates;
using MaceTech.API.Watering.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MaceTech.API.Watering.Infrastructure.Persistence.EFC.Repositories;

public class WateringLogRepository(AppDbContext context) : BaseRepository<WateringLog>(context), IWateringLogRepository
{
    public async Task<IEnumerable<WateringLog>> FindByDeviceIdAndDateRangeAsync(string deviceId, DateTime? fromDate, DateTime? toDate)
    {
        var query = Context.Set<WateringLog>().Where(w => w.DeviceId == deviceId);

        if (fromDate.HasValue)
        {
            query = query.Where(w => w.Timestamp >= fromDate.Value);
        }
        if (toDate.HasValue)
        {
            query = query.Where(w => w.Timestamp <= toDate.Value);
        }

        return await query.OrderByDescending(w => w.Timestamp).ToListAsync();
    }
}