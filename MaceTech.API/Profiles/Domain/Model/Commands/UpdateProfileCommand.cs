namespace MaceTech.API.Profiles.Domain.Model.Commands;

public record UpdateProfileCommand(
    string Uid, string FirstName, string LastName,
    string Street, string Number, string City, string PostalCode, string Country,
    string CountryCode, string PhoneNumber
    );