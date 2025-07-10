using MaceTech.API.Watering.Domain.Model.Aggregates;
using MaceTech.API.Watering.Domain.Model.Entity;

namespace MaceTech.API.Watering.Interfaces.ACL;

public interface IWateringContextFacade
{
    Task<WateringHistory> FetchWateringLogsByDeviceIdAndDateRange(long deviceId, DateTime? fromDate, DateTime? toDate);
}