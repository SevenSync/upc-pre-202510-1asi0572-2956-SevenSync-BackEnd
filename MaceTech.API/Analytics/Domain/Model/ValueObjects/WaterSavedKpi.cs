namespace MaceTech.API.Analytics.Domain.Model.ValueObjects;

public record WaterSavedKpi(
    long DeviceId,
    DateTime Date,
    double StandardWateringMl,
    double ActualWateringMl,
    double WaterSavedMl
);