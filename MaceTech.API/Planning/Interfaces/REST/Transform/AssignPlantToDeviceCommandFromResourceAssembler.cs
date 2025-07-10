using MaceTech.API.Planning.Domain.Model.Commands;
using MaceTech.API.Planning.Interfaces.REST.Resources;

namespace MaceTech.API.Planning.Interfaces.REST.Transform;

public static class AssignPlantToDeviceCommandFromResourceAssembler
{
    public static AssignPlantToDeviceCommand ToCommandFromResource(AssignPlantToDeviceResource resource, long deviceId)
    {
        return new AssignPlantToDeviceCommand(deviceId, resource.PlantId);
    }
}