using MaceTech.API.Shared.Domain.Repositories;
using MaceTech.API.Watering.Domain.Model.Aggregates;
using MaceTech.API.Watering.Domain.Model.Commands;
using MaceTech.API.Watering.Domain.Repositories;
using MaceTech.API.Watering.Domain.Services.CommandServices;

namespace MaceTech.API.Watering.Application.Internal.CommandServices;

public class WateringLogCommandService(IWateringLogRepository wateringLogRepository, IUnitOfWork unitOfWork) : IWateringLogCommandService
{
    public async Task<WateringLog> Handle(CreateWateringLogCommand command)
    {
        var log = new WateringLog(command);
        await wateringLogRepository.AddAsync(log);
        await unitOfWork.CompleteAsync();
        return log;
    }
}