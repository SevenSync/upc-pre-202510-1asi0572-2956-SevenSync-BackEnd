namespace MaceTech.API.Profiles.Interfaces.REST.Resources;

public record CreateProfileResource(
    string FirstName, string LastName,
    string Street, string BuildingNumber, string City, string PostalCode, string Country,
    string CountryCode, string PhoneNumber
    );