namespace MaceTech.API.Watering.Domain.Model.Queries;

public record GetWateringHistoryByDeviceIdAndDateRangeQuery(long DeviceId, DateTime? From, DateTime? To);