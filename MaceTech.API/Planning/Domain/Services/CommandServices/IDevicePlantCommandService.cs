using MaceTech.API.Planning.Domain.Model.Aggregates;
using MaceTech.API.Planning.Domain.Model.Commands;

namespace MaceTech.API.Planning.Domain.Services.CommandServices;


/// <summary>
/// Contrato para el servicio que maneja los comandos relacionados con la planta de un dispositivo.
/// </summary>
public interface IDevicePlantCommandService
{
    /// <summary>
    /// Maneja el comando para asignar una planta a un dispositivo.
    /// </summary>
    /// <param name="command">El comando con el ID del dispositivo y el ID de la planta.</param>
    /// <returns>La entidad DevicePlant creada o actualizada.</returns>
    Task<DevicePlant?> Handle(AssignPlantToDeviceCommand command);
    Task<DevicePlant?> Handle(UpdateDevicePlantThresholdsCommand command);
}