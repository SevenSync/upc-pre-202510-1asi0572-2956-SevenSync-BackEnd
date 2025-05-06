namespace MaceTech.API.Profiles.Domain.Model.ValueObjects;

public record PersonName(string FirstName, string LastName)
{
    public string FullName => $"{FirstName} {LastName}";
    
    public PersonName() : this(string.Empty, string.Empty) { }
}