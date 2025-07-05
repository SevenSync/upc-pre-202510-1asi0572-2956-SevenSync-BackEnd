using MaceTech.API.Analytics.Domain.Model.Aggregates;
using MaceTech.API.Analytics.Domain.Model.ValueObjects;
using MaceTech.API.Analytics.Domain.Services;
using MaceTech.API.Analytics.Interfaces.ACL.DTOs;

namespace MaceTech.API.Analytics.Application.Internal.DomainServices;

public class AnalyticsDomainService : IAnalyticsDomainService
{
    
    private const double StandardDailyWateringMl = 500.0;

    public PotComparisonData CalculateComparisonForDevice(string deviceId, IEnumerable<PotRecord> records, IEnumerable<AlertDataDto> alerts, IEnumerable<WateringLogDataDto> wateringLogs)
    {
        var potRecords = records.ToList();
        if (potRecords.Count == 0)
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
    
    public WaterSavedKpi CalculateWaterSavedKpi(string deviceId, DateTime date, IEnumerable<WateringLogDataDto> dailyWateringLogs)
    {
        // Calculamos el total de agua usada realmente por MaceTech en el día.
        var actualWateringMl = dailyWateringLogs.Select(log => log.WaterVolumeMl).Sum();

        // Calculamos el ahorro.
        var waterSavedMl = StandardDailyWateringMl - actualWateringMl;

        return new WaterSavedKpi(
            deviceId,
            date,
            StandardDailyWateringMl,
            actualWateringMl,
            waterSavedMl > 0 ? waterSavedMl : 0 // El ahorro no puede ser negativo
        );
    }
}