namespace MaceTech.API.Analytics.Interfaces.REST.Resources;

public record PotRecordResource(long Id, string DeviceId, float Temperature, float Humidity, int Light, float Salinity, float Ph, DateTime CreatedAt);