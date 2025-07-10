namespace MaceTech.API.Planning.Domain.Model.Commands;

public record AssignPlantToDeviceCommand(long DeviceId, long PlantId);