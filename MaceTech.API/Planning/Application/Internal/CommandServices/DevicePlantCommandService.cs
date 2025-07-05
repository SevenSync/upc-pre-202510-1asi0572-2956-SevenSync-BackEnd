using MaceTech.API.Planning.Domain.Model.Aggregates;
using MaceTech.API.Planning.Domain.Model.Commands;
using MaceTech.API.Planning.Domain.Repositories;
using MaceTech.API.Planning.Domain.Services.CommandServices;
using MaceTech.API.Shared.Domain.Repositories;

namespace MaceTech.API.Planning.Application.Internal.CommandServices;

public class DevicePlantCommandService(
    IDevicePlantRepository devicePlantRepository, 
    IPlantRepository plantRepository, 
    IUnitOfWork unitOfWork) : IDevicePlantCommandService
{
    public async Task<DevicePlant?> Handle(AssignPlantToDeviceCommand command)
    {
        var plant = await plantRepository.FindByIdAsync(command.PlantId);
        if (plant == null)
        {
            Console.WriteLine($"Error: Plant with ID '{command.PlantId}' not found.");
            return null;
        }

        var existingDevicePlant = await devicePlantRepository.FindByDeviceIdAsync(command.DeviceId);

        if (existingDevicePlant != null)
        {
            existingDevicePlant.UpdatePlant(plant);
            devicePlantRepository.Update(existingDevicePlant);
        }
        else
        {
            existingDevicePlant = new DevicePlant(command.DeviceId, plant);
            await devicePlantRepository.AddAsync(existingDevicePlant);
        }
        
        await unitOfWork.CompleteAsync();
        return existingDevicePlant;
    }

    public async Task<DevicePlant?> Handle(UpdateDevicePlantThresholdsCommand command)
    {
        var devicePlant = await devicePlantRepository.FindByDeviceIdAsync(command.DeviceId);

        if (devicePlant == null)
        {
            return null; 
        }
        
        try
        {
            devicePlant.UpdateCustomThresholds(command.NewThresholds);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Validation Error: {ex.Message}");
            throw; 
        }

        devicePlantRepository.Update(devicePlant);
        
        await unitOfWork.CompleteAsync();

        return devicePlant;
    }
}