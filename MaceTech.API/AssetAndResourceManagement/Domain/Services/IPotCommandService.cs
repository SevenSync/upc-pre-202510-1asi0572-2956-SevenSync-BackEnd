using MaceTech.API.AssetAndResourceManagement.Domain.Model.Aggregates;
using MaceTech.API.AssetAndResourceManagement.Domain.Model.Commands;

namespace MaceTech.API.AssetAndResourceManagement.Domain.Services;

public interface IPotCommandService
{
    public Task<Pot?> Handle(CreatePotCommand command);
    public Task<Pot?> Handle(PotAssignmentCommand command);
    public Task<Pot?> Handle(UpdatePotMetricsCommand command);
    public Task<Pot?> Handle(DeletePotCommand command);
    public Task<Pot?> Handle(UnassignPotFromUserCommand command);
}