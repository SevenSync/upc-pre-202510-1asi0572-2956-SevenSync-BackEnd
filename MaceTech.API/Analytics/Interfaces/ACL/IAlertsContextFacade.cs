using MaceTech.API.Analytics.Domain.Model.Aggregates;
using MaceTech.API.Analytics.Interfaces.ACL.DTOs;

namespace MaceTech.API.Analytics.Interfaces.ACL;

public interface IAlertsContextFacade
{
    // El método ahora devuelve una colección del DTO, no de la entidad.
    Task<IEnumerable<AlertDataDto>> FetchAlertsByDeviceIdAndDateRange(string deviceId, DateTime fromDate, DateTime toDate);
}