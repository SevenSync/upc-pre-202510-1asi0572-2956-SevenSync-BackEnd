namespace MaceTech.API.Profiles.Domain.Model.ValueObjects;

public record PersonAddress(string Street, string BuildingNumber,  string City, string PostalCode, string Country)
{
    public string FullAddress =>  $"{Street} {BuildingNumber}, {City}, {PostalCode}, {Country}";

    public PersonAddress() : this(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty) { }
}