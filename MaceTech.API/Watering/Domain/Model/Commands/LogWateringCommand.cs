namespace MaceTech.API.Watering.Domain.Model.Commands;

public record LogWateringCommand(string DeviceId, int DurationSeconds, float InitialHumidity, float FinalHumidity, bool Success);
