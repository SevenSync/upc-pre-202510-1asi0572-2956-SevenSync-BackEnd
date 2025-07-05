using MaceTech.API.Analytics.Domain.Model.Queries;
using MaceTech.API.Analytics.Domain.Model.ValueObjects;
using MaceTech.API.Analytics.Domain.Services;
using MaceTech.API.Analytics.Domain.Services.QueriesServices;
using MaceTech.API.Analytics.Interfaces.ACL.Services;

namespace MaceTech.API.Analytics.Application.Internal.QueryServices;

public class KpiQueryService(
    WateringLogContextFacade wateringLogContextFacade,
    IAnalyticsDomainService analyticsDomainService) : IKpiQueryService
{
    
    public async Task<WaterSavedKpi> Handle(GetWaterSavedKpiQuery query)
    {
        var fromDate = query.Date.Date;
        var toDate = fromDate.AddDays(1).AddTicks(-1);

        var dailyWateringLogs = await wateringLogContextFacade.GetWateringLogHistoryForDevice(query.DeviceId, fromDate, toDate);

        var kpi = analyticsDomainService.CalculateWaterSavedKpi(query.DeviceId, query.Date.Date, dailyWateringLogs);

        return kpi;
    }
}