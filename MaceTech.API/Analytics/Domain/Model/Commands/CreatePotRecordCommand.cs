namespace MaceTech.API.Analytics.Domain.Model.Commands;

public record CreatePotRecordCommand(long DeviceId, float Temperature, float Humidity, float Light, float Salinity, float Ph);