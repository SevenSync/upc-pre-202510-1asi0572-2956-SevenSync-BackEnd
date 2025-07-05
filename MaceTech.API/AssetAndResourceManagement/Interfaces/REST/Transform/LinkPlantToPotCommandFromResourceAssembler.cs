using MaceTech.API.AssetAndResourceManagement.Domain.Model.Commands;

namespace MaceTech.API.AssetAndResourceManagement.Interfaces.REST.Transform;

public static class LinkPlantToPotCommandFromResourceAssembler
{
    public static LinkPlantCommand ToCommandFromResource(long potId, string uid, AssignPotToUserResource resource)
    {
        return new LinkPlantCommand(potId, uid, potId);
    }
}