namespace MaceTech.API.Profiles.Domain.Model.ValueObjects;

public record class EmailAddress(string Address)
{
    public EmailAddress(): this(string.Empty) { }
}