using MaceTech.API.IAM.Domain.Model.Commands;
using MaceTech.API.IAM.Interfaces.REST.Resources;

namespace MaceTech.API.IAM.Interfaces.REST.Transform;

public static class SignInCommandFromResourceAssembler
{
    public static SignInCommand ToCommandFromResource(SignInResource resource)
    {
        return new SignInCommand(resource.Email, resource.Password);
    }
}