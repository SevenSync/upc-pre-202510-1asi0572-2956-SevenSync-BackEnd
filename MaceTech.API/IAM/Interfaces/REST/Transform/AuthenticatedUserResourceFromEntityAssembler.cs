using MaceTech.API.IAM.Domain.Model.Aggregates;
using MaceTech.API.IAM.Interfaces.REST.Resources;

namespace MaceTech.API.IAM.Interfaces.REST.Transform;

public static class AuthenticatedUserResourceFromEntityAssembler
{
    public static AuthenticatedUserResource ToResourceFromEntity(User entity, string token)
    {
        return new AuthenticatedUserResource(entity.Uid, entity.Email, token);
    }
}