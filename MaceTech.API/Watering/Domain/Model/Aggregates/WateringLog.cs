namespace MaceTech.API.Watering.Domain.Model.Aggregates;

public class WateringLog
{
    public int Id { get; private set; }
    public string DeviceId { get; private set; }
    public DateTime Timestamp { get; private set; }
    public int DurationSeconds { get; private set; }
    public double WaterVolumeMl { get; private set; }
    
    public float InitialHumidity { get; private set; } 
    public float FinalHumidity { get; private set; }
    public string Result { get; private set; }
    
    private const double FlowRateMlPerSecond = 415;

    public WateringLog() { /* EF Core */ DeviceId = string.Empty; Result = string.Empty; }

    public WateringLog(string deviceId, int durationSeconds, float initialHumidity, float finalHumidity, bool success)
    {
        DeviceId = deviceId;
        Timestamp = DateTime.UtcNow;
        DurationSeconds = durationSeconds;
        WaterVolumeMl = durationSeconds * FlowRateMlPerSecond;
        InitialHumidity = initialHumidity;
        FinalHumidity = finalHumidity;
        Result = success ? "Success" : "Failed";
    }
}