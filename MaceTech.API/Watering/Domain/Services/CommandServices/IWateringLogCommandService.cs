using MaceTech.API.Watering.Domain.Model.Aggregates;
using MaceTech.API.Watering.Domain.Model.Commands;

namespace MaceTech.API.Watering.Domain.Services.CommandServices;

public interface IWateringLogCommandService
{
    Task<WateringLog> Handle(LogWateringCommand command);
}