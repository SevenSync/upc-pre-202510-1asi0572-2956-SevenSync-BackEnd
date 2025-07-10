using MaceTech.API.Planning.Domain.Model.Aggregates;
using MaceTech.API.Planning.Interfaces.REST.Resources;

namespace MaceTech.API.Planning.Interfaces.REST.Transform;

public static class DevicePlantResourceFromEntityAssembler
{
    public static DevicePlantResource ToResourceFromEntity(DevicePlant entity)
    {

        return new DevicePlantResource(
            entity.Plant.OptimalParameters.Temperature.Min,
            entity.Plant.OptimalParameters.Temperature.Max,
            entity.Plant.OptimalParameters.Humidity.Min,
            entity.Plant.OptimalParameters.Humidity.Max,
            entity.Plant.OptimalParameters.Light.Min,
            entity.Plant.OptimalParameters.Light.Max,
            entity.Plant.OptimalParameters.Salinity.Min,
            entity.Plant.OptimalParameters.Salinity.Max,
            entity.Plant.OptimalParameters.Ph.Min,
            entity.Plant.OptimalParameters.Ph.Max
        );
    }
}