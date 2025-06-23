using MaceTech.API.Watering.Domain.Model.Aggregates.MaceTech.API.Watering.Domain.Model.Aggregates;
using MaceTech.API.Watering.Domain.Model.Queries;

namespace MaceTech.API.Watering.Domain.Services.QueryServices;

public interface IWateringLogQueryService
{
    Task<IEnumerable<WateringLog>> Handle(GetWateringHistoryByDeviceIdQuery query);
}