using MaceTech.API.Planning.Domain.Model.Aggregates;
using MaceTech.API.Planning.Interfaces.REST.Resources;

namespace MaceTech.API.Planning.Interfaces.REST.Transform;

public static class DevicePlantResourceFromEntityAssembler
{
    public static DevicePlantResource ToResourceFromEntity(DevicePlant entity)
    {

        return new DevicePlantResource(
            entity.Id,
            entity.DeviceId,
            entity.PlantId,
            entity.Plant.CommonName
        );
    }
}