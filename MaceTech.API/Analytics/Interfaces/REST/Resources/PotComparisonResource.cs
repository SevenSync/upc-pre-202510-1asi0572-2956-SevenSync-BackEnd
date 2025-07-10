namespace MaceTech.API.Analytics.Interfaces.REST.Resources;

public record PotComparisonResource(
    long DeviceId,
    double WeeklyAvgTemperature,
    double WeeklyAvgHumidity,
    double WeeklyAvgPh,
    double TotalWaterVolumeMl,
    int CriticalAlertsCount
);