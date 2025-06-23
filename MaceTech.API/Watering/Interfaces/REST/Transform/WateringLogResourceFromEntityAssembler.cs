using MaceTech.API.Watering.Domain.Model.Aggregates.MaceTech.API.Watering.Domain.Model.Aggregates;
using MaceTech.API.Watering.Interfaces.REST.Resources;

namespace MaceTech.API.Watering.Interfaces.REST.Transform;

public static class WateringLogResourceFromEntityAssembler
{
    public static WateringLogResource ToResourceFromEntity(WateringLog entity)
    {
        return new WateringLogResource(
            entity.Timestamp,
            entity.DurationSeconds,
            Math.Round(entity.WaterVolumeMl),
            entity.InitialHumidity,
            entity.FinalHumidity,
            entity.Result
        );
    }
}