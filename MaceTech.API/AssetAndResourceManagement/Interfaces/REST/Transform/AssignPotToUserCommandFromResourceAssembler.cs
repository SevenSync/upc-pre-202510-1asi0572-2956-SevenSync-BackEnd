using MaceTech.API.AssetAndResourceManagement.Domain.Model.Commands;
using MaceTech.API.AssetAndResourceManagement.Interfaces.REST.Resources;

namespace MaceTech.API.AssetAndResourceManagement.Interfaces.REST.Transform;

public static class AssignPotToUserCommandFromResourceAssembler
{
    public static AssignPotToUserCommand ToCommandFromResource(string uid, AssignPotToUserResource resource)
    {
        return new AssignPotToUserCommand(uid, resource.PotId, resource.Name, resource.Location);
    }
}