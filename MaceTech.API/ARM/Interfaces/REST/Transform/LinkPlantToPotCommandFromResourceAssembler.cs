using MaceTech.API.ARM.Domain.Model.Commands;

namespace MaceTech.API.ARM.Interfaces.REST.Transform;

public static class LinkPlantToPotCommandFromResourceAssembler
{
    public static LinkPlantCommand ToCommandFromResource(long potId, string uid, long plantId)
    {
        return new LinkPlantCommand(potId, uid, potId);
    }
}