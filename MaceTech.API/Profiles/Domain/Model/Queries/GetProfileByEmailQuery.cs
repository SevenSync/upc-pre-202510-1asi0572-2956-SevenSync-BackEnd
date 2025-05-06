using MaceTech.API.Profiles.Domain.Model.ValueObjects;

namespace MaceTech.API.Profiles.Domain.Model.Queries;

public record GetProfileByEmailQuery(EmailAddress Email);