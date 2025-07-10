using MaceTech.API.Shared.Domain.Repositories;
using MaceTech.API.Watering.Domain.Model.Aggregates;
using MaceTech.API.Watering.Domain.Model.Entity;

namespace MaceTech.API.Watering.Domain.Repositories;

public interface IWateringLogRepository : IBaseRepository<WateringLog>
{
    Task<WateringHistory> FindByDeviceIdAndDateRangeAsync(long deviceId, DateTime? fromDate, DateTime? toDate);
    Task<WateringHistory> FindByDeviceIdAsync(long deviceId);
}