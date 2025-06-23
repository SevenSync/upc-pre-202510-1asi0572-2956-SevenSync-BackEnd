using MaceTech.API.Analytics.Domain.Model.Aggregates;
using MaceTech.API.Analytics.Domain.Repositories;
using MaceTech.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using MaceTech.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MaceTech.API.Analytics.Infrastructure.Persistence.EFC.Repositories;

public class PotRecordRepository(AppDbContext context) : IPotRecordRepository
{
    public async Task AddAsync(PotRecord potRecord)
    {
        await context.Set<PotRecord>().AddAsync(potRecord);
    }

    public async Task<PotRecord?> FindLatestByDeviceIdAsync(string deviceId)
    {
        return await context.Set<PotRecord>()
            .Where(r => r.DeviceId == deviceId)
            .OrderByDescending(r => r.CreatedAt)
            .FirstOrDefaultAsync();
    }
    
    public async Task<IEnumerable<PotRecord>> GetRecordsByDeviceIdAndDateRangeAsync(string deviceId, DateTime fromDate, DateTime toDate)
    {
        return await context.Set<PotRecord>()
            .Where(p => p.DeviceId == deviceId && p.CreatedAt >= fromDate && p.CreatedAt <= toDate)
            .ToListAsync();
    }
}
