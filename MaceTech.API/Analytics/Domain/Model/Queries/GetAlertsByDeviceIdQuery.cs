namespace MaceTech.API.Analytics.Domain.Model.Queries;

public record GetAlertsByDeviceIdQuery(string DeviceId, DateTime? FromDate, DateTime? ToDate, string? AlertType);
