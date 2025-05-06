using MaceTech.API.Profiles.Domain.Model.Commands;
using MaceTech.API.Profiles.Interfaces.REST.Resources;

namespace MaceTech.API.Profiles.Interfaces.REST.Transform;

public static class CreateProfileCommandFromResourceAssembler
{
    public static CreateProfileCommand ToCommandFromResource(CreateProfileResource resource)
    {
        return new CreateProfileCommand(
            resource.FirstName, resource.LastName, resource.Email, 
            resource.Street, resource.Number, resource.City, resource.PostalCode, resource.Country,
            resource.CountryCode, resource.PhoneNumber
            );
    }
}