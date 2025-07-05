using MaceTech.API.Planning.Domain.Model.Aggregates;
using MaceTech.API.Planning.Domain.Model.Commands;

namespace MaceTech.API.Planning.Domain.Services.CommandServices;

public interface IDevicePlantCommandService
{
    Task<DevicePlant?> Handle(AssignPlantToDeviceCommand command);
    Task<DevicePlant?> Handle(UpdateDevicePlantThresholdsCommand command);
}