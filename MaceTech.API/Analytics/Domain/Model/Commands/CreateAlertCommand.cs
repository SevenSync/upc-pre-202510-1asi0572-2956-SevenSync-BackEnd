namespace MaceTech.API.Analytics.Domain.Model.Commands;

public record CreateAlertCommand(long DeviceId, string AlertType, float Value);
