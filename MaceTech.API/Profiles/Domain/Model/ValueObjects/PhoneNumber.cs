namespace MaceTech.API.Profiles.Domain.Model.ValueObjects;

public record PhoneNumber(string CountryCode, string Number)
{
    public string FullNumber => $"{CountryCode} {Number}";
    
    public PhoneNumber() : this(string.Empty, string.Empty) { }
}