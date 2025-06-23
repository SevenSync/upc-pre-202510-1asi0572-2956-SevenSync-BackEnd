namespace MaceTech.API.Profiles.Interfaces.ACL;

public interface IProfilesContextFacade
{
    Task<long> CreateProfile(
        string uid,
        string firstName, string lastName,
        string street, string number, string city, string postalCode, string country,
        string countryCode, string phoneNumber);
}