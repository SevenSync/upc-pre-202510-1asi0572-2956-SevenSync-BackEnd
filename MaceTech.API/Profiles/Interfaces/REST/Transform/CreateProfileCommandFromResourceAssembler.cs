using MaceTech.API.Profiles.Domain.Model.Commands;
using MaceTech.API.Profiles.Interfaces.REST.Resources;

namespace MaceTech.API.Profiles.Interfaces.REST.Transform;

public static class CreateProfileCommandFromResourceAssembler
{
    public static CreateProfileCommand ToCommandFromResource(string uid, CreateProfileResource resource)
    {
        return new CreateProfileCommand(
            uid, resource.FirstName, resource.LastName,
            resource.Street, resource.Number, resource.City, resource.PostalCode, resource.Country,
            resource.CountryCode, resource.PhoneNumber
            );
    }
}