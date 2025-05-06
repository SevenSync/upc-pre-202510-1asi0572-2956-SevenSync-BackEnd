using MaceTech.API.Profiles.Domain.Model.Commands;
using MaceTech.API.Profiles.Domain.Model.Queries;
using MaceTech.API.Profiles.Domain.Model.ValueObjects;
using MaceTech.API.Profiles.Domain.Services;

namespace MaceTech.API.Profiles.Interfaces.ACL.Services;

public class ProfilesContextFacade(
    IProfileCommandService profileCommandService, 
    IProfileQueryService profileQueryService
    ) : IProfilesContextFacade
{
    public async Task<long> CreateProfile(
        string firstName, string lastName, string email, string street, string number, string city,
        string postalCode, string country, string countryCode, string phoneNumber
        )
    {
        var createProfileCommand = new CreateProfileCommand(
            firstName, lastName, email, street, number, city, postalCode, country, countryCode, phoneNumber
        );
        var profile = await profileCommandService.Handle(createProfileCommand);

        return profile?.Id ?? 0;
    }


    public async Task<long> FetchProfileIdByEmail(string email)
    {
        var getProfileByEmailQuery = new GetProfileByEmailQuery(new EmailAddress(email));
        var profile = await profileQueryService.Handle(getProfileByEmailQuery);

        return profile?.Id ?? 0;
    }
}