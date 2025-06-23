using MaceTech.API.Profiles.Domain.Model.Aggregates;
using MaceTech.API.Profiles.Domain.Model.ValueObjects;
using MaceTech.API.Profiles.Domain.Repositories;
using MaceTech.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using MaceTech.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MaceTech.API.Profiles.Infrastructure.Persistence.EFC.Repositories;

public class ProfileRepository(
    AppDbContext context
    ) : BaseRepository<Profile>(context), IProfileRepository
{
    public Task<Profile?> FindProfileByIdAsync(long id)
    {
        return Context.Set<Profile>().Where(p => p.Id == id).FirstOrDefaultAsync();
    }

    public Task<Profile?> FindProfileByUidAsync(string uid)
    {
        return Context.Set<Profile>().Where(p => p.Uid == uid).FirstOrDefaultAsync();
    }
}