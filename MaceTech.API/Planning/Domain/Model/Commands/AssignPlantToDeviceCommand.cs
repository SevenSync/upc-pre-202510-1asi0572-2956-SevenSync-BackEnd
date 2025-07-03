namespace MaceTech.API.Planning.Domain.Model.Commands;

/// <summary>
/// Comando que representa la intención de asignar una planta del catálogo a un dispositivo.
/// </summary>
public record AssignPlantToDeviceCommand(string DeviceId, int PlantId);