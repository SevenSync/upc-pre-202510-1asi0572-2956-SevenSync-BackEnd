using MaceTech.API.Planning.Domain.Model.ValueObjects;

namespace MaceTech.API.Planning.Domain.Model.Commands;

public record UpdateDevicePlantThresholdsCommand(long DeviceId, OptimalParameters NewThresholds);