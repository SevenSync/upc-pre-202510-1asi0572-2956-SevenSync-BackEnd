namespace MaceTech.API.Analytics.Domain.Model.Queries;

public record GetAlertsByDeviceIdAndDateRangeQuery(string DeviceId, DateTime From, DateTime To);
