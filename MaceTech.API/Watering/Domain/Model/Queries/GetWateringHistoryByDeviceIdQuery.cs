namespace MaceTech.API.Watering.Domain.Model.Queries;

public record GetWateringHistoryByDeviceIdQuery(string DeviceId, DateTime? From, DateTime? To);