using MaceTech.API.Profiles.Domain.Model.Aggregates;
using MaceTech.API.Shared.Domain.Repositories;

namespace MaceTech.API.Profiles.Domain.Repositories;

public interface IProfileRepository : IBaseRepository<Profile>
{
    public Task<Profile?> FindProfileByIdAsync(long id);
    public Task<Profile?> FindProfileByUidAsync(string uid);
}