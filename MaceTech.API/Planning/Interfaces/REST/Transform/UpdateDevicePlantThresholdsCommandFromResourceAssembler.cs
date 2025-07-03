using MaceTech.API.Planning.Domain.Model.Aggregates;
using MaceTech.API.Planning.Domain.Model.Commands;
using MaceTech.API.Planning.Domain.Model.ValueObjects;
using MaceTech.API.Planning.Interfaces.REST.Resources;

namespace MaceTech.API.Planning.Interfaces.REST.Transform;

public static class UpdateDevicePlantThresholdsCommandFromResourceAssembler
{
    // Este ensamblador es más complejo porque debe combinar los datos del recurso
    // con los datos existentes de la entidad que no se están modificando.
    public static UpdateDevicePlantThresholdsCommand ToCommandFromResource(UpdateDevicePlantThresholdsResource resource, DevicePlant existingSettings)
    {
        var updatedParameters = new OptimalParameters(
            resource.TemperaturaAmbiente,
            resource.Humedad,
            existingSettings.CustomThresholds.Luminosidad, // Mantenemos los valores existentes
            existingSettings.CustomThresholds.SalinidadSuelo,
            existingSettings.CustomThresholds.PhSuelo
        );
        
        return new UpdateDevicePlantThresholdsCommand(existingSettings.DeviceId, updatedParameters);
    }
}