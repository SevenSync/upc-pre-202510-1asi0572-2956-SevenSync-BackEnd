using MaceTech.API.Shared.Domain.Repositories;
using MaceTech.API.Watering.Domain.Model.Aggregates;

namespace MaceTech.API.Watering.Domain.Repositories;

public interface IWateringLogRepository : IBaseRepository<WateringLog>
{
    Task<IEnumerable<WateringLog>> FindByDeviceIdAndDateRangeAsync(string deviceId, DateTime? fromDate, DateTime? toDate);
}