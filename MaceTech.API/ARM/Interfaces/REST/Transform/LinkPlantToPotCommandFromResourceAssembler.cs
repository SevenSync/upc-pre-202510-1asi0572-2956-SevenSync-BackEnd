using MaceTech.API.ARM.Domain.Model.Commands;
using MaceTech.API.ARM.Interfaces.REST.Resources;

namespace MaceTech.API.ARM.Interfaces.REST.Transform;

public static class LinkPlantToPotCommandFromResourceAssembler
{
    public static LinkPlantCommand ToCommandFromResource(long potId, string uid, LinkPlantToPotResource resource)
    {
        return new LinkPlantCommand(potId, uid, resource.PlantId);
    }
}