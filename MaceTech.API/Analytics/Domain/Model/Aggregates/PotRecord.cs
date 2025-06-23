namespace MaceTech.API.Analytics.Domain.Model.Aggregates;

// Representa una única lectura de telemetría.
public class PotRecord
{
    public long Id { get; private set; }
    public string DeviceId { get; private set; }
    public float Temperature { get; private set; }
    public float Humidity { get; private set; }
    public int Light { get; private set; }
    public float Salinity { get; private set; }
    public float Ph { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public PotRecord() // Requerido por Entity Framework Core
    {
        DeviceId = string.Empty;
    }

    public PotRecord(string deviceId, float temperature, float humidity, int light, float salinity, float ph)
    {
        DeviceId = deviceId;
        Temperature = temperature;
        Humidity = humidity;
        Light = light;
        Salinity = salinity;
        Ph = ph;
        CreatedAt = DateTime.UtcNow;
    }
}