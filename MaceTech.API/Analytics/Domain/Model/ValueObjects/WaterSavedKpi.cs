namespace MaceTech.API.Analytics.Domain.Model.ValueObjects;

public record WaterSavedKpi(
    string DeviceId,
    DateTime Date,
    double StandardWateringMl,
    double ActualWateringMl,
    double WaterSavedMl
);