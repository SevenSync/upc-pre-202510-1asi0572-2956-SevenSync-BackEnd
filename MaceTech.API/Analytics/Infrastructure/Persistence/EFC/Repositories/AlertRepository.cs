using MaceTech.API.Analytics.Domain.Model.Aggregates;
using MaceTech.API.Analytics.Domain.Repositories;
using MaceTech.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using MaceTech.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MaceTech.API.Analytics.Infrastructure.Persistence.EFC.Repositories;

public class AlertRepository(AppDbContext context) : BaseRepository<Alert>(context), IAlertRepository
{
    public async Task<IEnumerable<Alert>> FindByDeviceIdAndDateRangeAsync(long deviceId, DateTime fromDate, DateTime toDate)
    {
        return await Context.Set<Alert>()
            .Where(a => a.DeviceId == deviceId && a.Timestamp >= fromDate && a.Timestamp <= toDate)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<Alert>> FindByDeviceIdWithFiltersAsync(long deviceId, DateTime? fromDate, DateTime? toDate, string? alertType)
    {
        var query = Context.Set<Alert>().Where(a => a.DeviceId == deviceId);

        if (fromDate.HasValue)
        {
            query = query.Where(a => a.Timestamp >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            query = query.Where(a => a.Timestamp <= toDate.Value);
        }

        if (!string.IsNullOrWhiteSpace(alertType))
        {
            query = query.Where(a => a.AlertType == alertType);
        }
        return await query.OrderByDescending(a => a.Timestamp).ToListAsync();
    }
}