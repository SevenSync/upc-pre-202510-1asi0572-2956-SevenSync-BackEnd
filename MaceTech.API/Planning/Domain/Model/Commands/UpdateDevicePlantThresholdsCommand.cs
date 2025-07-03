using MaceTech.API.Planning.Domain.Model.ValueObjects;

namespace MaceTech.API.Planning.Domain.Model.Commands;

/// <summary>
/// Comando para actualizar los umbrales de una planta asignada a un dispositivo.
/// </summary>
public record UpdateDevicePlantThresholdsCommand(string DeviceId, OptimalParameters NewThresholds);