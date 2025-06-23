using MaceTech.API.Analytics.Domain.Model.Aggregates;

namespace MaceTech.API.Analytics.Domain.Repositories;

public interface IPotRecordRepository
{
    Task AddAsync(PotRecord potRecord);
    Task<PotRecord?> FindLatestByDeviceIdAsync(string deviceId);
    Task<IEnumerable<PotRecord>> GetRecordsByDeviceIdAndDateRangeAsync(string deviceId, DateTime fromDate, DateTime toDate);
}