using MaceTech.API.Profiles.Domain.Model.Aggregates;
using MaceTech.API.Profiles.Domain.Model.Queries;
namespace MaceTech.API.Profiles.Domain.Services;

public interface IProfileQueryService
{
    public Task<IEnumerable<Profile>> Handle(GetAllProfilesQuery query);
    public Task<Profile?> Handle(GetProfileByUidQuery query);
    public Task<bool> Handle(HasProfileQuery query);
}