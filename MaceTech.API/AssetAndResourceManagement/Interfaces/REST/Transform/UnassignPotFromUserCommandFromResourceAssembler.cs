using MaceTech.API.AssetAndResourceManagement.Domain.Model.Commands;

namespace MaceTech.API.AssetAndResourceManagement.Interfaces.REST.Transform;

public class UnassignPotFromUserCommandFromResourceAssembler
{
    public static UnassignPotFromUserCommand ToCommandFromResource(long potId, string uid)
    {
        return new UnassignPotFromUserCommand(potId, uid);
    }
}