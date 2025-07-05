using MaceTech.API.Planning.Domain.Model.Aggregates;
using MaceTech.API.Planning.Interfaces.REST.Resources;

namespace MaceTech.API.Planning.Interfaces.REST.Transform;

public static class DevicePlantResourceFromEntityAssembler
{
    public static DevicePlantResource ToResourceFromEntity(DevicePlant entity)
    {
        var thresholds = new ThresholdsResource(
            entity.CustomThresholds.TemperaturaAmbiente.Min,
            entity.CustomThresholds.TemperaturaAmbiente.Max,
            entity.CustomThresholds.Humedad.Min,
            entity.CustomThresholds.Humedad.Max
        );

        return new DevicePlantResource(
            entity.Id,
            entity.DeviceId,
            entity.PlantId,
            entity.Plant.CommonName,
            thresholds
        );
    }
}