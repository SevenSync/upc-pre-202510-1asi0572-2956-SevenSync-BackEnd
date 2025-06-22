using MaceTech.API.Analytics.Domain.Model.Queries;
using MaceTech.API.Analytics.Domain.Model.ValueObjects;
using MaceTech.API.Analytics.Domain.Repositories;
using MaceTech.API.Analytics.Domain.Services.QueriesServices;
using MaceTech.API.Analytics.Interfaces.ACL;

namespace MaceTech.API.Analytics.Application.Internal.QueryServices;

public class AnalyticsQueryService(
    IPotRecordRepository potRecordRepository,
    IAlertsContextFacade alertsContextFacade,
    IWateringContextFacade wateringContextFacade) : IAnalyticsQueryService
{
    public async Task<IEnumerable<PotComparisonData>> Handle(GetPotComparisonQuery query)
    {
        var comparisonResults = new List<PotComparisonData>();
        var toDate = DateTime.UtcNow;
        var fromDate = toDate.AddDays(-7);

        foreach (var deviceId in query.DeviceIds)
        {
            var recordsTask = potRecordRepository.GetRecordsByDeviceIdAndDateRangeAsync(deviceId, fromDate, toDate);
            var alertsTask = alertsContextFacade.FetchAlertsByDeviceIdAndDateRange(deviceId, fromDate, toDate);
            var wateringTask = wateringContextFacade.FetchWateringLogsByDeviceIdAndDateRange(deviceId, fromDate, toDate);

            await Task.WhenAll(recordsTask, alertsTask, wateringTask);
            
            // Materializamos los resultados en listas para trabajar con ellas en memoria.
            var weeklyRecords = (await recordsTask).ToList();
            var weeklyAlerts = (await alertsTask).ToList();
            var weeklyWatering = (await wateringTask).ToList();

            // Ahora la comprobación .Any() y las operaciones .Average()/.Sum()
            // se ejecutan sobre la lista en memoria, de forma segura.
            if (weeklyRecords.Count == 0) continue;
            if (weeklyAlerts.Count == 0 && weeklyWatering.Count == 0)
            {
                // Si no hay alertas ni riego, no hay datos que comparar.
                continue;
            }
            var result = new PotComparisonData(
                DeviceId: deviceId,
                WeeklyAvgTemperature: weeklyRecords.Average(r => r.Temperature),
                WeeklyAvgHumidity: weeklyRecords.Average(r => r.Humidity),
                WeeklyAvgPh: weeklyRecords.Average(r => r.Ph),
                
                TotalWaterVolumeMl: weeklyWatering.Sum(w => (float)w.WaterVolumeMl),
                
                CriticalAlertsCount: weeklyAlerts.Count(a => a.GeneratedRecommendation.Urgency == "Crítica")
            );
            comparisonResults.Add(result);
        }
        return comparisonResults;
    }
}