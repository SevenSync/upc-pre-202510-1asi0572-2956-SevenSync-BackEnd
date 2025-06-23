using MaceTech.API.IAM.Domain.Model.Commands;
using MaceTech.API.IAM.Interfaces.REST.Resources;

namespace MaceTech.API.IAM.Interfaces.REST.Transform;

public static class SignUpCommandFromResourceAssembler
{
    public static SignUpCommand ToCommandFromResource(string userId, SignUpResource resource)
    {
        return new SignUpCommand(userId, resource.Email, resource.Password);
    }
}