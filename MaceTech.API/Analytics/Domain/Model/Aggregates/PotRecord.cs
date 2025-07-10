namespace MaceTech.API.Analytics.Domain.Model.Aggregates;

public class PotRecord
{
    public long Id { get; }
    public long DeviceId { get; private set; }
    public float Temperature { get; private set; }
    public float Humidity { get; private set; }
    public float Light { get; private set; }
    public float Salinity { get; private set; }
    public float Ph { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public PotRecord()
    {
        DeviceId = 0;
    }

    public PotRecord(long deviceId, float temperature, float humidity, float light, float salinity, float ph)
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