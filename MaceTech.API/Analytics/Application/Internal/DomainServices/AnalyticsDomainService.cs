using MaceTech.API.Analytics.Domain.Model.Aggregates;
using MaceTech.API.Analytics.Domain.Model.ValueObjects;
using MaceTech.API.Analytics.Domain.Services;
using MaceTech.API.Analytics.Interfaces.ACL.DTOs;

namespace MaceTech.API.Analytics.Application.Internal.DomainServices;

public class AnalyticsDomainService : IAnalyticsDomainService
{
    public PotComparisonData CalculateComparisonForDevice(string deviceId, IEnumerable<PotRecord> records, IEnumerable<AlertDataDto> alerts, IEnumerable<WateringLogDataDto> wateringLogs)
    {
        var potRecords = records.ToList();
        if (!potRecords.Any())
        {
            return new PotComparisonData(deviceId, 0, 0, 0, 0, 0);
        }

        var alertsList = alerts.ToList();
        var wateringLogsList = wateringLogs.ToList();

        return new PotComparisonData(
            DeviceId: deviceId,
            WeeklyAvgTemperature: potRecords.Average(r => r.Temperature),
            WeeklyAvgHumidity: potRecords.Average(r => r.Humidity),
            WeeklyAvgPh: potRecords.Average(r => r.Ph),
            // Ahora trabajamos con las propiedades del DTO
            TotalWaterVolumeMl: wateringLogsList.Select(w => w.WaterVolumeMl).Sum(),
            CriticalAlertsCount: alertsList.Count(a => a.Urgency == "Crítica")
        );
    }
}