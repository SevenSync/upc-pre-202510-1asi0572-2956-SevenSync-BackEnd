using MaceTech.API.ARM.Domain.Model.Commands;

namespace MaceTech.API.ARM.Interfaces.REST.Transform;

public static class UnlinkPlantFromPotCommandResourceAssembler
{
    public static UnlinkPlantCommand ToCommandFromResource(long potId, string uid)
    {
        return new UnlinkPlantCommand(potId, uid);
    }
}