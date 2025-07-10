using MaceTech.API.Watering.Domain.Model.Entity;
using MaceTech.API.Watering.Interfaces.REST.Resources;

namespace MaceTech.API.Watering.Interfaces.REST.Transform;

public class WateringHistoryResourceFromEntityAssembler
{
    public static WateringHistoryResource ToResourceFromEntity(WateringHistory entity)
    {
        return new WateringHistoryResource
        (
            entity.Id,
            entity.deviceId,
            entity.Historial.Select(WateringLogResourceFromEntityAssembler.ToResourceFromEntity),
            entity.TotalEjecuciones,
            entity.PromedioAguaMl
        );
    }
}