using MaceTech.API.AssetAndResourceManagement.Domain.Model.Commands;
using MaceTech.API.AssetAndResourceManagement.Interfaces.REST.Resources;

namespace MaceTech.API.AssetAndResourceManagement.Interfaces.REST.Transform;

public static class AssignPotToUserCommandFromResourceAssembler
{
    public static AssignPotToUserCommand ToCommandFromResource(long potId, string uid, AssignPotToUserResource resource)
    {
        return new AssignPotToUserCommand(potId, uid, resource.Name, resource.Location);
    }
}