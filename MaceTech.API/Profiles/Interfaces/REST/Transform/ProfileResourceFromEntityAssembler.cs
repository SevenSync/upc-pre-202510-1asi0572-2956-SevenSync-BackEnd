using MaceTech.API.Profiles.Domain.Model.Aggregates;
using MaceTech.API.Profiles.Interfaces.REST.Resources;

namespace MaceTech.API.Profiles.Interfaces.REST.Transform;

public static class ProfileResourceFromEntityAssembler
{
    public static ProfileResource ToResourceFromEntity(Profile entity)
    {
        return new ProfileResource(entity.Id, entity.FullName, entity.StreetAddress, entity.FullPhoneNumber);
    }
}