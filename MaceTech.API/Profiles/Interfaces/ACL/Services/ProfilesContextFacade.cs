using MaceTech.API.Profiles.Domain.Model.Commands;
using MaceTech.API.Profiles.Domain.Services;

namespace MaceTech.API.Profiles.Interfaces.ACL.Services;

public class ProfilesContextFacade(
    IProfileCommandService profileCommandService, 
    IProfileQueryService profileQueryService
    ) : IProfilesContextFacade
{
    public async Task<long> CreateProfile(
        string uid, string firstName, string lastName, string street, string number, string city,
        string postalCode, string country, string countryCode, string phoneNumber
        )
    {
        var createProfileCommand = new CreateProfileCommand(
            uid, firstName, lastName, street, number, city, postalCode, country, countryCode, phoneNumber
        );
        var profile = await profileCommandService.Handle(createProfileCommand);

        return profile?.Id ?? 0;
    }
}