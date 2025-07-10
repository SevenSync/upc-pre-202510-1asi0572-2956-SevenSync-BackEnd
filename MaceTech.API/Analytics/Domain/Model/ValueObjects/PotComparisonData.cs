namespace MaceTech.API.Analytics.Domain.Model.ValueObjects;

public record PotComparisonData(
    long DeviceId,
    double WeeklyAvgTemperature,
    double WeeklyAvgHumidity,
    double WeeklyAvgPh,
    double TotalWaterVolumeMl,
    int CriticalAlertsCount
);