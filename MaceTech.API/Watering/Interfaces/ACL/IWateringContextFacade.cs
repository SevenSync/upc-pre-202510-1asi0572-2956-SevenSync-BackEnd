using MaceTech.API.Watering.Domain.Model.Aggregates;

namespace MaceTech.API.Watering.Interfaces.ACL;

public interface IWateringContextFacade
{
    Task<IEnumerable<WateringLog>> FetchWateringLogsByDeviceIdAndDateRange(string deviceId, DateTime? fromDate, DateTime? toDate);
}