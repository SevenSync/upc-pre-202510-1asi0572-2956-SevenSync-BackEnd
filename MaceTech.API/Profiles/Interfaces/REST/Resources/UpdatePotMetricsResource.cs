namespace MaceTech.API.Profiles.Interfaces.REST.Resources;

public record UpdatePotMetricsResource(
    long PotId,
    float BatteryLevel,
    float WaterLevel,
    float Humidity,
    float Luminance,
    float Temperature,
    float Ph,
    float Salinity
    );