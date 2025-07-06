using MaceTech.API.ARM.Domain.Model.Aggregates;
using MaceTech.API.ARM.Domain.Model.Commands;

namespace MaceTech.API.ARM.Domain.Services;

public interface IPotCommandService
{
    public Task<Pot?> Handle(CreatePotCommand command);
    public Task<Pot?> Handle(PotAssignmentCommand command);
    public Task<Pot?> Handle(UpdatePotMetricsCommand command);
    public Task<Pot?> Handle(DeletePotCommand command);
    public Task<Pot?> Handle(UnassignPotFromUserCommand command);
}