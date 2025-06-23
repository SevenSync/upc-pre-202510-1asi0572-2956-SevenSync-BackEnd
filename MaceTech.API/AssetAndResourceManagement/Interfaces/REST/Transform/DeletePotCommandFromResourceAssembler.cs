using MaceTech.API.AssetAndResourceManagement.Domain.Model.Commands;

namespace MaceTech.API.AssetAndResourceManagement.Interfaces.REST.Transform;

public static class DeletePotCommandFromResourceAssembler
{
    public static DeletePotCommand ToCommandFromResource(long potId)
    {
        return new DeletePotCommand(potId);
    }
}