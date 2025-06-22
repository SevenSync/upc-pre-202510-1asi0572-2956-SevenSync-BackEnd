using MaceTech.API.Analytics.Domain.Model.Commands;
using MaceTech.API.Analytics.Interfaces.REST.Resources;

namespace MaceTech.API.Analytics.Interfaces.REST.Transform;

public static class CreatePotRecordCommandFromResourceAssembler
{
    public static CreatePotRecordCommand ToCommandFromResource(CreatePotRecordResource resource)
    {
        return new CreatePotRecordCommand(resource.DeviceId, resource.Temperature, resource.Humidity, resource.Light, resource.Salinity, resource.Ph);
    }
}