

using MaceTech.API.Analytics.Domain.Model.Queries;
using MaceTech.API.Analytics.Domain.Model.ValueObjects;
using MaceTech.API.Analytics.Domain.Repositories;
using MaceTech.API.Analytics.Domain.Services;
using MaceTech.API.Analytics.Domain.Services.QueriesServices;
using MaceTech.API.Analytics.Interfaces.ACL;

namespace MaceTech.API.Analytics.Application.Internal.QueryServices;

public class AnalyticsQueryService(
    IPotRecordRepository potRecordRepository,
    IAlertsContextFacade alertsContextFacade,
    IWateringLogContextFacade wateringLogContextFacade,
    IAnalyticsDomainService analyticsDomainService) : IAnalyticsQueryService
{
    public async Task<IEnumerable<PotComparisonData>> Handle(GetPotComparisonQuery query)
    {
        var comparisonResults = new List<PotComparisonData>();
        var toDate = DateTime.UtcNow;
        var fromDate = toDate.AddDays(-7);

        foreach (var deviceId in query.DeviceIds)
        {
            // La lógica de orquestación ahora es mucho más legible.
            var recordsTask = potRecordRepository.GetRecordsByDeviceIdAndDateRangeAsync(deviceId, fromDate, toDate);
            var alertsTask = alertsContextFacade.FetchAlertsByDeviceIdAndDateRange(deviceId, fromDate, toDate);
            var wateringTask = wateringLogContextFacade.GetWateringLogHistoryForDevice(deviceId, fromDate, toDate);

            await Task.WhenAll(recordsTask, alertsTask, wateringTask);
            
            var weeklyRecords = await recordsTask;
            var weeklyAlerts = await alertsTask;
            var weeklyWatering = await wateringTask;
            
            var result = analyticsDomainService.CalculateComparisonForDevice(
                deviceId,
                weeklyRecords,
                weeklyAlerts,
                weeklyWatering
            );
            
            comparisonResults.Add(result);
        }
        return comparisonResults;
    }
}