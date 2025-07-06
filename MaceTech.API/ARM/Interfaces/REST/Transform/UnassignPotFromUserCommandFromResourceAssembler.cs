using MaceTech.API.ARM.Domain.Model.Commands;

namespace MaceTech.API.ARM.Interfaces.REST.Transform;

public class UnassignPotFromUserCommandFromResourceAssembler
{
    public static UnassignPotFromUserCommand ToCommandFromResource(long potId, string uid)
    {
        return new UnassignPotFromUserCommand(potId, uid);
    }
}