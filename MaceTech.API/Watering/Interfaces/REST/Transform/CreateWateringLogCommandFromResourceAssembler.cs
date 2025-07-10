using MaceTech.API.Watering.Domain.Model.Commands;
using MaceTech.API.Watering.Interfaces.REST.Resources;

namespace MaceTech.API.Watering.Interfaces.REST.Transform;

public static class CreateWateringLogCommandFromResourceAssembler
{
    public static CreateWateringLogCommand ToCommandFromResource(CreateWateringLogResource resource, long deviceId)
    {
        return new CreateWateringLogCommand(
            deviceId,
            (int)resource.DurationSeconds,
            resource.WasSuccessful,
            resource.Reason
        );
    }
}