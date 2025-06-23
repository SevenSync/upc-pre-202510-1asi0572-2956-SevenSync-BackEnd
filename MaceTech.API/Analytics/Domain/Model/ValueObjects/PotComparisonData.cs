namespace MaceTech.API.Analytics.Domain.Model.ValueObjects;

public record PotComparisonData(
    string DeviceId,
    double WeeklyAvgTemperature,
    double WeeklyAvgHumidity,
    double WeeklyAvgPh,
    double TotalWaterVolumeMl,
    int CriticalAlertsCount
);