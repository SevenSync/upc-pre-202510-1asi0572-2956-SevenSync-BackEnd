using MaceTech.API.Analytics.Domain.Model.Aggregates;
using MaceTech.API.Analytics.Domain.Model.ValueObjects;
using MaceTech.API.Analytics.Interfaces.ACL.DTOs;

namespace MaceTech.API.Analytics.Domain.Services;

public interface IAnalyticsDomainService
{
    PotComparisonData CalculateComparisonForDevice(
        string deviceId, 
        IEnumerable<PotRecord> records, 
        IEnumerable<AlertDataDto> alerts, 
        IEnumerable<WateringLogDataDto> wateringLogs
    );
    
    WaterSavedKpi CalculateWaterSavedKpi(
        string deviceId,
        DateTime date,
        IEnumerable<WateringLogDataDto> dailyWateringLogs
    );
}