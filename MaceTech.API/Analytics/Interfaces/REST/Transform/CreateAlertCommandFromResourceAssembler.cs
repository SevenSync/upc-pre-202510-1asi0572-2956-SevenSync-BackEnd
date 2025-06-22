using MaceTech.API.Analytics.Domain.Model.Commands;
using MaceTech.API.Analytics.Interfaces.REST.Resources;

namespace MaceTech.API.Analytics.Interfaces.REST.Transform;

public static class CreateAlertCommandFromResourceAssembler
{
    public static CreateAlertCommand ToCommandFromResource(CreateAlertResource resource, string deviceId)
    {
        return new CreateAlertCommand(deviceId, resource.AlertType, resource.Value);
    }
}