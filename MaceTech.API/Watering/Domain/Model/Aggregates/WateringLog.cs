namespace MaceTech.API.Watering.Domain.Model.Aggregates;

namespace MaceTech.API.Watering.Domain.Model.Aggregates;

public class WateringLog
{
    public int Id { get; private set; }
    public string DeviceId { get; private set; }
    public DateTime Timestamp { get; private set; }
    public int DurationSeconds { get; private set; }
    public double WaterVolumeMl { get; private set; }
    
    // Dato obtenido del contexto de Analytics
    public float InitialHumidity { get; private set; } 
    public float FinalHumidity { get; private set; }
    public string Result { get; private set; }
    
    private const double FLOW_RATE_ML_PER_SECOND = 415; // Caudal promedio de la válvula

    public WateringLog() { /* EF Core */ DeviceId = string.Empty; Result = string.Empty; }

    public WateringLog(string deviceId, int durationSeconds, float initialHumidity, float finalHumidity, bool success)
    {
        DeviceId = deviceId;
        Timestamp = DateTime.UtcNow;
        DurationSeconds = durationSeconds;
        WaterVolumeMl = durationSeconds * FLOW_RATE_ML_PER_SECOND;
        InitialHumidity = initialHumidity;
        FinalHumidity = finalHumidity;
        Result = success ? "éxito" : "fallo";
    }
}