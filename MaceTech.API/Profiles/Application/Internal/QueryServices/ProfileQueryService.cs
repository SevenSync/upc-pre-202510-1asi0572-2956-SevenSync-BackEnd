using MaceTech.API.Profiles.Domain.Model.Aggregates;
using MaceTech.API.Profiles.Domain.Model.Queries;
using MaceTech.API.Profiles.Domain.Repositories;
using MaceTech.API.Profiles.Domain.Services;

namespace MaceTech.API.Profiles.Application.Internal.QueryServices;

public class ProfileQueryService(IProfileRepository profileRepository) : IProfileQueryService
{
    public async Task<IEnumerable<Profile>> Handle(GetAllProfilesQuery query)
    {
        return await profileRepository.ListAsync();
    }
    
    public async Task<Profile?> Handle(GetProfileByUidQuery query)
    {
        return await profileRepository.FindProfileByUidAsync(query.Uid);
    }
    
    public async Task<bool> Handle(HasProfileQuery query)
    {
        var result = await profileRepository.FindProfileByUidAsync(query.Uid); 
        if (result is null) return false;
        return true;
    }
}