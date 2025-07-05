using MaceTech.API.Planning.Domain.Model.Aggregates;
using MaceTech.API.Planning.Domain.Model.Commands;
using MaceTech.API.Planning.Domain.Model.ValueObjects;
using MaceTech.API.Planning.Interfaces.REST.Resources;

namespace MaceTech.API.Planning.Interfaces.REST.Transform;

public static class UpdateDevicePlantThresholdsCommandFromResourceAssembler
{

    public static UpdateDevicePlantThresholdsCommand ToCommandFromResource(UpdateDevicePlantThresholdsResource resource, DevicePlant existingSettings)
    {
        var updatedParameters = new OptimalParameters(
            resource.TemperaturaAmbiente,
            resource.Humedad,
            resource.Luminosidad,
            resource.SalinidadSuelo,
            resource.PhSuelo
            );
        
        return new UpdateDevicePlantThresholdsCommand(existingSettings.DeviceId, updatedParameters);
    }
}