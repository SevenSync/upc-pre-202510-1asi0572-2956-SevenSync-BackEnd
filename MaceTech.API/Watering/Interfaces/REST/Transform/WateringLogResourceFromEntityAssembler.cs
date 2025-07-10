using MaceTech.API.Watering.Domain.Model.Aggregates;
using MaceTech.API.Watering.Interfaces.REST.Resources;

namespace MaceTech.API.Watering.Interfaces.REST.Transform;

public class WateringLogResourceFromEntityAssembler
{
    public static WateringLogResource ToResourceFromEntity(WateringLog entity)
    {
        return new WateringLogResource
        (
            entity.Id,
            entity.DeviceId,
            entity.DurationSeconds,
            entity.WasSuccessful,
            entity.Reason
        );
    }
}