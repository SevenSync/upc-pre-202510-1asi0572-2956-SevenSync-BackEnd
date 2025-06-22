namespace MaceTech.API.Analytics.Domain.Model.Commands;

public record CreateAlertCommand(string DeviceId, string AlertType, float Value);
