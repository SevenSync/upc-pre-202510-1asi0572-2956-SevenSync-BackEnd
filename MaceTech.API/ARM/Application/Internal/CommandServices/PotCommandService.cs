using MaceTech.API.ARM.Domain.Model.Aggregates;
using MaceTech.API.ARM.Domain.Model.Commands;
using MaceTech.API.ARM.Domain.Repositories;
using MaceTech.API.ARM.Domain.Services;
using MaceTech.API.Shared.Domain.Repositories;

namespace MaceTech.API.ARM.Application.Internal.CommandServices;

public class PotCommandService(
    IPotRepository repository,
    IUnitOfWork unitOfWork
    ) : IPotCommandService
{
    public async Task<Pot?> Handle(CreatePotCommand command)
    {
        var pot = new Pot();
        try
        {
            await repository.AddAsync(pot);
            await unitOfWork.CompleteAsync();
            return pot;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    public async Task<Pot?> Handle(PotAssignmentCommand command)
    {
        var pot = await repository.FindPotByIdAsync(command.PotId);
        if (pot == null) return null;

        try
        {
            pot.AssignUser(command);
            repository.Update(pot);
            await unitOfWork.CompleteAsync();
            return pot;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    public async Task<Pot?> Handle(UpdatePotMetricsCommand command)
    {
        var pot = await repository.FindPotByIdAsync(command.PotId);
        if (pot == null) return null;

        try
        {
            pot.UpdateMetrics(command);
            repository.Update(pot);
            await unitOfWork.CompleteAsync();
            return pot;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    public async Task<Pot?> Handle(DeletePotCommand command)
    {
        var pot = await repository.FindPotByIdAsync(command.PotId);
        if (pot == null) return null;

        try
        {
            pot.Delete();
            repository.Update(pot);
            await unitOfWork.CompleteAsync();
            return pot;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    public async Task<Pot?> Handle(UnassignPotFromUserCommand command)
    {
        var pot = await repository.FindPotByIdAsync(command.PotId);
        if (pot == null) return null;

        try
        {
            pot.UnassignPlant();
            repository.Update(pot);
            await unitOfWork.CompleteAsync();
            return pot;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}