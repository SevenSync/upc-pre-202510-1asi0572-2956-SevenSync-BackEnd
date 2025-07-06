using MaceTech.API.ARM.Domain.Model.Commands;

namespace MaceTech.API.ARM.Interfaces.REST.Transform;

public static class DeletePotCommandFromResourceAssembler
{
    public static DeletePotCommand ToCommandFromResource(long potId)
    {
        return new DeletePotCommand(potId);
    }
}