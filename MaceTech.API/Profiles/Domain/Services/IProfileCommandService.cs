using MaceTech.API.Profiles.Domain.Model.Aggregates;
using MaceTech.API.Profiles.Domain.Model.Commands;

namespace MaceTech.API.Profiles.Domain.Services;

public interface IProfileCommandService
{
    public Task<Profile?> Handle(CreateProfileCommand command);
    public Task Handle(UpdateProfileCommand command);
}