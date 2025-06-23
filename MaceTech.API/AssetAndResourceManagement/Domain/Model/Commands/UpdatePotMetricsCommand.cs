namespace MaceTech.API.AssetAndResourceManagement.Domain.Model.Commands;

public record UpdatePotMetricsCommand(
    long PotId,
    float BatteryLevel,
    float WaterLevel,
    float Humidity,
    float Luminance,
    float Temperature,
    float Ph,
    float Salinity
    );