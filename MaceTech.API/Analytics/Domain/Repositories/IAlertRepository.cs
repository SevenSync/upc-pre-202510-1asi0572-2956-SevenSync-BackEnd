using MaceTech.API.Analytics.Domain.Model.Aggregates;
using MaceTech.API.Shared.Domain.Repositories;

namespace MaceTech.API.Analytics.Domain.Repositories;

public interface IAlertRepository : IBaseRepository<Alert>
{
    Task<IEnumerable<Alert>> FindByDeviceIdAndDateRangeAsync(string deviceId, DateTime fromDate, DateTime toDate);
    Task<IEnumerable<Alert>> FindByDeviceIdWithFiltersAsync(string deviceId, DateTime? fromDate, DateTime? toDate, string? alertType);
}