using MaceTech.API.AssetAndResourceManagement.Domain.Model.Commands;
using MaceTech.API.Profiles.Interfaces.REST.Resources;

namespace MaceTech.API.AssetAndResourceManagement.Interfaces.REST.Transform;

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