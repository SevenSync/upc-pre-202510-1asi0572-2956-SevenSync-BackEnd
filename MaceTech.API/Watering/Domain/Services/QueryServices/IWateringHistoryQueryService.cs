using MaceTech.API.Watering.Domain.Model.Aggregates;
using MaceTech.API.Watering.Domain.Model.Entity;
using MaceTech.API.Watering.Domain.Model.Queries;

namespace MaceTech.API.Watering.Domain.Services.QueryServices;

public interface IWateringHistoryQueryService
{
    Task<WateringHistory> Handle(GetWateringHistoryByDeviceIdAndDateRangeQuery query);
    Task<WateringHistory> Handle(GetWateringHistoryByDeviceIdQuery query); 
}