using MaceTech.API.Watering.Domain.Model.Commands;

namespace MaceTech.API.Watering.Domain.Model.Aggregates;

public class WateringLog
{
    public long Id { get; }
    public long DeviceId { get; private set; }
    public DateTime Timestamp { get; private set; }
    public int DurationSeconds { get; private set; }
    public double WaterVolumeMl { get; private set; }
    public bool WasSuccessful { get; private set; }
    public string Reason { get; private set; }

    public WateringLog()
    {
        DeviceId = 0;
        Timestamp = DateTime.Now;
        DurationSeconds = 0;
        WaterVolumeMl = 0.0;
        WasSuccessful = false;
        Reason = string.Empty;
    }

    public WateringLog(CreateWateringLogCommand command)
    {
        DeviceId = command.DeviceId;
        Timestamp = DateTime.UtcNow;
        DurationSeconds = command.DurationSeconds;
        WaterVolumeMl = command.DurationSeconds * 415; // 415 ml per second
        WasSuccessful = command.WasSuccessful;
        Reason = command.Reason;
    }

    public WateringLog(long deviceId, int durationSeconds, bool wasSuccessful, string reason)
    {
        DeviceId = deviceId;
        Timestamp = DateTime.UtcNow;
        DurationSeconds = durationSeconds;
        WasSuccessful = wasSuccessful;
        Reason = reason;
        WaterVolumeMl = durationSeconds * 415;
    }
}