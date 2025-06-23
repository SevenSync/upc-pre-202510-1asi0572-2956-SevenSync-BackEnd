using MaceTech.API.IAM.Domain.Model.Aggregates;
using MaceTech.API.IAM.Interfaces.REST.Resources;

namespace MaceTech.API.IAM.Interfaces.REST.Transform;

public static class UserResourceFromEntityAssembler
{
    public static UserResource ToResourceFromEntity(User entity)
    {
        return new UserResource(entity.Uid, entity.Email);
    }
}