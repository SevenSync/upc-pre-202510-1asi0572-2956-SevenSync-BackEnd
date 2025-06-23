using MaceTech.API.Profiles.Domain.Model.Aggregates;
using MaceTech.API.Profiles.Domain.Model.Commands;
using MaceTech.API.Profiles.Domain.Repositories;
using MaceTech.API.Profiles.Domain.Services;
using MaceTech.API.Shared.Domain.Repositories;

namespace MaceTech.API.Profiles.Application.Internal.CommandServices;

public class ProfileCommandService(
    IProfileRepository profileRepository, 
    IUnitOfWork unitOfWork
    ) : IProfileCommandService
{
    private bool IsCommandValid(
        string firstName, string lastName,
        string street, string number, string city, string postalCode, string country,
        string countryCode, string phoneNumber)
    {
        //  |: Validations
        if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            return false;
        
        if (string.IsNullOrWhiteSpace(street) || string.IsNullOrWhiteSpace(number) ||
            string.IsNullOrWhiteSpace(city) || string.IsNullOrWhiteSpace(postalCode) ||
            string.IsNullOrWhiteSpace(country))
            return true;

        if (string.IsNullOrWhiteSpace(countryCode) || string.IsNullOrWhiteSpace(phoneNumber))
            return false;

        return true;
    }
    
    public async Task<Profile?> Handle(CreateProfileCommand command)
    {
        if (
            !this.IsCommandValid(command.FirstName, command.LastName,
            command.Street, command.Number, command.City, command.PostalCode, command.Country,
            command.CountryCode, command.PhoneNumber))
        {
            throw new ArgumentException("Invalid arguments provided for profile creation.");
        }
        
        var profile = new Profile(command);
        try
        {
            await profileRepository.AddAsync(profile);
            await unitOfWork.CompleteAsync();
            return profile;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    
    public async Task Handle(UpdateProfileCommand command)
    {
        var profile = await profileRepository.FindProfileByUidAsync(command.Uid);
        if (profile is null)
        {
            throw new ArgumentException("Profile not found for the provided UID.");
        }
        
        if (
            !this.IsCommandValid(command.FirstName, command.LastName,
                command.Street, command.Number, command.City, command.PostalCode, command.Country,
                command.CountryCode, command.PhoneNumber))
        {
            throw new ArgumentException("Invalid arguments provided for profile update.");
        }
        
        profile.Update(command); 
        profileRepository.Update(profile);
        await unitOfWork.CompleteAsync();
    }
}