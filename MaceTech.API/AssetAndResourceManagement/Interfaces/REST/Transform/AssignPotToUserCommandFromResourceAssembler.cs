using MaceTech.API.AssetAndResourceManagement.Domain.Model.Commands;
using MaceTech.API.AssetAndResourceManagement.Interfaces.REST.Resources;

namespace MaceTech.API.AssetAndResourceManagement.Interfaces.REST.Transform;

public static class AssignPotToUserCommandFromResourceAssembler
{
    public static PotAssignmentCommand ToCommandFromResource(long potId, string uid, AssignPotToUserResource resource)
    {
        return new PotAssignmentCommand(potId, uid, resource.Name, resource.Location);
    }
}