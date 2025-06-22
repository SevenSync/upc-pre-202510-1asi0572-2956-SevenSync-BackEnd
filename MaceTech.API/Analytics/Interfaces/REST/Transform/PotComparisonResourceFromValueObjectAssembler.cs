using MaceTech.API.Analytics.Domain.Model.ValueObjects;
using MaceTech.API.Analytics.Interfaces.REST.Resources;

namespace MaceTech.API.Analytics.Interfaces.REST.Transform;

public static class PotComparisonResourceFromValueObjectAssembler
{
    public static PotComparisonResource ToResourceFromValueObject(PotComparisonData vo)
    {
        return new PotComparisonResource(
            vo.DeviceId,
            Math.Round(vo.WeeklyAvgTemperature, 2), // Redondeamos para una mejor presentación
            Math.Round(vo.WeeklyAvgHumidity, 2),
            Math.Round(vo.WeeklyAvgPh, 2),
            vo.TotalWaterVolumeMl,
            vo.CriticalAlertsCount
        );
    }
}