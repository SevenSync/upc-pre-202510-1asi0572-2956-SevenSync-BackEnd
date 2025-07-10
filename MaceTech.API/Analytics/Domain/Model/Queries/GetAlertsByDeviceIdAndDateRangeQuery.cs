namespace MaceTech.API.Analytics.Domain.Model.Queries;

public record GetAlertsByDeviceIdAndDateRangeQuery(long DeviceId, DateTime From, DateTime To);
