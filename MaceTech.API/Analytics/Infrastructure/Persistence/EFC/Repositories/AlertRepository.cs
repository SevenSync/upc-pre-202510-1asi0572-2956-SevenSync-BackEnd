using MaceTech.API.Analytics.Domain.Model.Aggregates;
using MaceTech.API.Analytics.Domain.Repositories;
using MaceTech.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using MaceTech.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MaceTech.API.Analytics.Infrastructure.Persistence.EFC.Repositories;

public class AlertRepository(AppDbContext context) : BaseRepository<Alert>(context), IAlertRepository
{
    public async Task<IEnumerable<Alert>> FindByDeviceIdAndDateRangeAsync(string deviceId, DateTime fromDate, DateTime toDate)
    {
        return await Context.Set<Alert>()
            .Where(a => a.DeviceId == deviceId && a.Timestamp >= fromDate && a.Timestamp <= toDate)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<Alert>> FindByDeviceIdWithFiltersAsync(string deviceId, DateTime? fromDate, DateTime? toDate, string? alertType)
    {
        // Empezamos con una consulta base que filtra por deviceId
        var query = Context.Set<Alert>().Where(a => a.DeviceId == deviceId);

        // Aplicamos los filtros opcionales de forma condicional
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

        // Ordenamos por fecha descendente y ejecutamos la consulta
        return await query.OrderByDescending(a => a.Timestamp).ToListAsync();
    }
}