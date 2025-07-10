namespace MaceTech.API.Analytics.Interfaces.REST.Resources;

public record WaterSavedKpiResource(
    long DeviceId,
    string Date,
    double StandardWateringMl,
    double ActualWateringMl,
    double WaterSavedMl,
    string Message
);