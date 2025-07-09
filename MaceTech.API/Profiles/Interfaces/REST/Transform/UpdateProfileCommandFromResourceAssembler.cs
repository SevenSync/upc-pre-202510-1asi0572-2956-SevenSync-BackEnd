using MaceTech.API.Profiles.Domain.Model.Commands;
using MaceTech.API.Profiles.Interfaces.REST.Resources;

namespace MaceTech.API.Profiles.Interfaces.REST.Transform;

public static class UpdateProfileCommandFromResourceAssembler
{
    public static UpdateProfileCommand ToCommandFromResource(string uid, UpdateProfileResource resource)
    {
        return new UpdateProfileCommand(
            uid, resource.FirstName, resource.LastName,
            resource.Street, resource.BuildingNumber, resource.City, resource.PostalCode, resource.Country,
            resource.CountryCode, resource.PhoneNumber
            );
    }
}