namespace MaceTech.API.Analytics.Domain.Model.Commands;

public record CreatePotRecordCommand(string DeviceId, float Temperature, float Humidity, int Light, float Salinity, float Ph);