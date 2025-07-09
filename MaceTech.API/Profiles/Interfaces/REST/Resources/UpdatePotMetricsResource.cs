namespace MaceTech.API.Profiles.Interfaces.REST.Resources;

public record UpdatePotMetricsResource(
    float BatteryLevel,
    float WaterLevel,
    float Humidity,
    float Luminance,
    float Temperature,
    float Ph,
    float Salinity
    );