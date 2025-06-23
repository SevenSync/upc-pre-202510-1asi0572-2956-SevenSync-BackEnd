namespace MaceTech.API.Profiles.Domain.Model.Commands;

public record CreateProfileCommand(
    string Uid, string FirstName, string LastName,
    string Street, string Number, string City, string PostalCode, string Country,
    string CountryCode, string PhoneNumber
    );