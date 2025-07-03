using MaceTech.API.Analytics.Domain.Model.Queries;
using MaceTech.API.Analytics.Domain.Model.ValueObjects;
using MaceTech.API.Analytics.Domain.Services;
using MaceTech.API.Analytics.Domain.Services.QueriesServices;
using MaceTech.API.Analytics.Interfaces.ACL.Services;

namespace MaceTech.API.Analytics.Application.Internal.QueryServices;

public class KpiQueryService(
    WateringLogContextFacade wateringContextFacade,
    IAnalyticsDomainService analyticsDomainService) : IKpiQueryService
{
    public async Task<WaterSavedKpi> Handle(GetWaterSavedKpiQuery query)
    {
        // 1. Definir el rango de tiempo para la consulta (un día completo).
        var fromDate = query.Date.Date; // Inicio del día (00:00:00)
        var toDate = fromDate.AddDays(1).AddTicks(-1); // Fin del día (23:59:59.999...)

        // 2. Usar la fachada para obtener los datos de riego de ese día.
        var dailyWateringLogs = await wateringContextFacade.FetchWateringLogsByDeviceIdAndDateRange(query.DeviceId, fromDate, toDate);

        // 3. Pasar los datos al servicio de dominio para que haga el cálculo.
        var kpi = analyticsDomainService.CalculateWaterSavedKpi(query.DeviceId, query.Date.Date, dailyWateringLogs);

        return kpi;
    }
}