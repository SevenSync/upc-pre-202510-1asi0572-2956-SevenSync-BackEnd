namespace MaceTech.API.Watering.Interfaces.REST.Resources;

public record WateringLogResource(
    long Id,
    long DeviceId,
    double DurationSeconds,
    bool WasSuccessful,
    string Reason
);