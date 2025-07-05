using MaceTech.API.Analytics.Domain.Model.Aggregates;
using MaceTech.API.Analytics.Domain.Model.Commands;
using MaceTech.API.Analytics.Domain.Repositories;
using MaceTech.API.Analytics.Domain.Services.CommandServices;
using MaceTech.API.Shared.Domain.Repositories;

namespace MaceTech.API.Analytics.Application.Internal.CommandServices;

public class PotRecordCommandService(IPotRecordRepository potRecordRepository, IUnitOfWork unitOfWork) : IPotRecordCommandService
{
    public async Task<PotRecord?> Handle(CreatePotRecordCommand command)
    {
        var potRecord = new PotRecord(command.DeviceId, command.Temperature, command.Humidity, command.Light, command.Salinity, command.Ph);
        try
        {
            await potRecordRepository.AddAsync(potRecord);
            await unitOfWork.CompleteAsync();
            return potRecord;
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occurred while creating the pot record: {e.Message}");
            return null;
        }
    }
}