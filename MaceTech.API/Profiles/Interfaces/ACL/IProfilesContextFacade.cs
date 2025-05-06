namespace MaceTech.API.Profiles.Interfaces.ACL;

public interface IProfilesContextFacade
{
    Task<long> CreateProfile(
        string firstName, string lastName, string email,
        string street, string number, string city, string postalCode, string country,
        string countryCode, string phoneNumber);
    
    Task<long> FetchProfileIdByEmail(string email);
}