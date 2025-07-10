using MaceTech.API.Analytics.Interfaces.REST.Resources;
using MaceTech.API.ARM.Domain.Model.Commands;

namespace MaceTech.API.ARM.Interfaces.REST.Transform;

public static class UpdatePotMetricsCommandFromResourceAssembler
{
    public static UpdatePotMetricsCommand ToCommandFromResource(long potId, UpdatePotMetricsResource resource)
    {
        return new UpdatePotMetricsCommand(
            potId,
            resource.BatteryLevel,
            resource.WaterLevel,
            resource.Humidity,
            resource.Luminance,
            resource.Temperature,
            resource.Ph,
            resource.Salinity);
    }
}