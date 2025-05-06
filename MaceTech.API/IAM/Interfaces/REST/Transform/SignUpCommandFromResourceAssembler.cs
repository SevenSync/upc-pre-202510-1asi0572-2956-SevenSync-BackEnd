using MaceTech.API.IAM.Domain.Model.Commands;
using MaceTech.API.IAM.Interfaces.REST.Resources;

namespace MaceTech.API.IAM.Interfaces.REST.Transform;

public static class SignUpCommandFromResourceAssembler
{
    public static SignUpCommand ToCommandFromResource(SignUpResource resource)
    {
        return new SignUpCommand(resource.Email, resource.Password);
    }
}