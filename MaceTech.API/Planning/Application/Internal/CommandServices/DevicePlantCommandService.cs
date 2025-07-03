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
        // 1. Validar que la planta que se quiere asignar existe en el catálogo.
        var plant = await plantRepository.FindByIdAsync(command.PlantId);
        if (plant == null)
        {
            // En un caso real, aquí podríamos lanzar una excepción personalizada.
            Console.WriteLine($"Error: Planta con ID {command.PlantId} no encontrada.");
            return null;
        }

        // 2. Verificar si ya existe una asignación para este dispositivo.
        var existingDevicePlant = await devicePlantRepository.FindByDeviceIdAsync(command.DeviceId);

        if (existingDevicePlant != null)
        {
            // Si ya existe, actualizamos la planta y reseteamos los umbrales a los óptimos.
            existingDevicePlant.UpdatePlant(plant);
            devicePlantRepository.Update(existingDevicePlant);
        }
        else
        {
            // Si no existe, creamos una nueva asignación.
            existingDevicePlant = new DevicePlant(command.DeviceId, plant);
            await devicePlantRepository.AddAsync(existingDevicePlant);
        }
        
        // 3. Guardar los cambios en la base de datos.
        await unitOfWork.CompleteAsync();
        return existingDevicePlant;
    }

    public async Task<DevicePlant?> Handle(UpdateDevicePlantThresholdsCommand command)
    {
        // 1. Buscar la asignación existente.
        var devicePlant = await devicePlantRepository.FindByDeviceIdAsync(command.DeviceId);

        if (devicePlant == null)
        {
            // No se puede actualizar algo que no existe.
            return null; 
        }
        
        // 2. Usar el método del agregado para actualizar los umbrales.
        // La validación de negocio ocurre dentro de la entidad.
        try
        {
            devicePlant.UpdateCustomThresholds(command.NewThresholds);
        }
        catch (ArgumentException ex)
        {
            // Si la validación falla, podríamos querer manejar el error aquí.
            Console.WriteLine($"Error de validación: {ex.Message}");
            // En una API real, lanzaríamos una excepción más específica para devolver un 400 Bad Request.
            throw; 
        }

        // 3. Informar a EF Core que la entidad ha sido modificada.
        devicePlantRepository.Update(devicePlant);
        
        // 4. Guardar los cambios.
        await unitOfWork.CompleteAsync();

        return devicePlant;
    }
}