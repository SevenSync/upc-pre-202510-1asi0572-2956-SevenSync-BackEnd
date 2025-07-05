using MaceTech.API.Analytics.Domain.Model.ValueObjects;
using MaceTech.API.Analytics.Interfaces.REST.Resources;

namespace MaceTech.API.Analytics.Interfaces.REST.Transform;

public static class WaterSavedKpiResourceFromValueObjectAssembler
{
    public static WaterSavedKpiResource ToResourceFromValueObject(WaterSavedKpi vo)
    {
        var message = $"El {vo.Date:dd/MM/yyyy}, MaceTech ahorró {vo.WaterSavedMl:F0}ml de agua.";
        return new WaterSavedKpiResource(
            vo.DeviceId,
            vo.Date.ToString("yyyy-MM-dd"),
            vo.StandardWateringMl,
            Math.Round(vo.ActualWateringMl),
            Math.Round(vo.WaterSavedMl),
            message
        );
    }
}