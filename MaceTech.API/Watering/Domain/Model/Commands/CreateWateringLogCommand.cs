namespace MaceTech.API.Watering.Domain.Model.Commands;

public record CreateWateringLogCommand(
    long DeviceId,
    int DurationSeconds,
    bool WasSuccessful,
    string Reason
);