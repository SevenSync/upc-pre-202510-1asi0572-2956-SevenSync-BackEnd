namespace MaceTech.API.Planning.Domain.Model.Commands;

public record AssignPlantToDeviceCommand(string DeviceId, int PlantId);