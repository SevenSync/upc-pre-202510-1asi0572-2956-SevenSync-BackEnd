using MaceTech.API.Profiles.Domain.Model.Commands;
using MaceTech.API.Profiles.Domain.Model.Enums;
using MaceTech.API.Profiles.Domain.Model.ValueObjects;

namespace MaceTech.API.Profiles.Domain.Model.Aggregates;

public partial class Profile
{
    //  @Properties
    public long Id { get; }
    public string Uid { get; private set; }
    public PersonName Name { get; private set; }
    public PersonAddress Address { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public int Status { get; private set; } = (int)ProfileStatus.Active;
    
    public string FullName => Name.FullName;
    public string StreetAddress => Address.FullAddress;
    public string FullPhoneNumber => PhoneNumber.FullNumber;
    
    //  @Constructors
    public Profile()
    {
        Uid = "";
        Name = new PersonName();
        Address = new PersonAddress();
        PhoneNumber = new PhoneNumber();
    }

    public Profile(
        string uid,
        string firstName, string lastName,
        string street, string number, string city, string postalCode, string country, 
        string countryCode, string phoneNumber
    )
    {
        Uid = uid;
        Name = new PersonName(firstName, lastName);
        Address = new PersonAddress(street, number, city, postalCode, country);
        PhoneNumber = new PhoneNumber(countryCode, phoneNumber);
    }
    
    public Profile(CreateProfileCommand command)
    {
        Uid = command.Uid;
        Name = new PersonName(command.FirstName, command.LastName);
        Address = new PersonAddress(command.Street, command.Number, command.City, command.PostalCode, command.Country);
        PhoneNumber = new PhoneNumber(command.CountryCode, command.PhoneNumber);
    }

    //  @Functions
    public void ChangeStatusToDeleted()
    {
        this.Status = (int)ProfileStatus.Deleted;
    }
    
    public void Update(UpdateProfileCommand command)
    {
        Name = new PersonName(command.FirstName, command.LastName);
        Address = new PersonAddress(command.Street, command.Number, command.City, command.PostalCode, command.Country);
        PhoneNumber = new PhoneNumber(command.CountryCode, command.PhoneNumber);
    }
}