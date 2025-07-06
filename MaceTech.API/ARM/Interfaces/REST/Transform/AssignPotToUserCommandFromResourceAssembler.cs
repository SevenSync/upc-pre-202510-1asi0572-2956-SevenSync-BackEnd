using MaceTech.API.ARM.Domain.Model.Commands;
using MaceTech.API.ARM.Interfaces.REST.Resources;

namespace MaceTech.API.ARM.Interfaces.REST.Transform;

public static class AssignPotToUserCommandFromResourceAssembler
{
    public static PotAssignmentCommand ToCommandFromResource(long potId, string uid, AssignPotToUserResource resource)
    {
        return new PotAssignmentCommand(potId, uid, resource.Name, resource.Location);
    }
}