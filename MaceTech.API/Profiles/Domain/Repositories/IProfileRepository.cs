using MaceTech.API.Profiles.Domain.Model.Aggregates;
using MaceTech.API.Profiles.Domain.Model.ValueObjects;
using MaceTech.API.Shared.Domain.Repositories;

namespace MaceTech.API.Profiles.Domain.Repositories;

public interface IProfileRepository : IBaseRepository<Profile>
{
    public Task<Profile?> FindProfileByEmailAsync(EmailAddress email);
}