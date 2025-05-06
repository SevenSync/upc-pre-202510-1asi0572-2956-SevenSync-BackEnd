using MaceTech.API.Profiles.Domain.Model.Commands;
using MaceTech.API.Profiles.Domain.Model.ValueObjects;

namespace MaceTech.API.Profiles.Domain.Model.Aggregates;

public partial class Profile
{
    //  @Properties
    public long Id { get; }
    public PersonName Name { get; private set; }
    public EmailAddress Email { get; private set; }
    public PersonAddress Address { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    
    public string FullName => Name.FullName;
    public string EmailAddress => Email.Address;
    public string StreetAddress => Address.FullAddress;
    public string FullPhoneNumber => PhoneNumber.FullNumber;
    
    //  @Constructors
    public Profile()
    {
        Name = new PersonName();
        Email = new EmailAddress();
        Address = new PersonAddress();
        PhoneNumber = new PhoneNumber();
    }

    public Profile(
        string firstName, string lastName, string email,
        string street, string number, string city, string postalCode, string country, 
        string countryCode, string phoneNumber
    )
    {
        Name = new PersonName(firstName, lastName);
        Email = new EmailAddress(email);
        Address = new PersonAddress(street, number, city, postalCode, country);
        PhoneNumber = new PhoneNumber(countryCode, phoneNumber);
    }
    
    public Profile(CreateProfileCommand command)
    {
        Name = new PersonName(command.FirstName, command.LastName);
        Email = new EmailAddress(command.Email);
        Address = new PersonAddress(command.Street, command.Number, command.City, command.PostalCode, command.Country);
        PhoneNumber = new PhoneNumber(command.CountryCode, command.PhoneNumber);
    }
}