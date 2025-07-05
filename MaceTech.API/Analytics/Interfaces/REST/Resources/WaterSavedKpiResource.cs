namespace MaceTech.API.Analytics.Interfaces.REST.Resources;

public record WaterSavedKpiResource(
    string DeviceId,
    string Date,
    double StandardWateringMl,
    double ActualWateringMl,
    double WaterSavedMl,
    string Message
);