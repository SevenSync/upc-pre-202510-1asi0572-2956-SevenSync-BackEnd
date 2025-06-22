using MaceTech.API.Analytics.Interfaces.ACL.DTOs;

namespace MaceTech.API.Analytics.Interfaces.ACL.Services;

public class WateringLogContextFacade(IWateringContextFacade wateringContextFacade) : IWateringLogContextFacade
{
    public async Task<IEnumerable<WateringLogDataDto>> GetWateringHistoryForDevice(string deviceId, DateTime fromDate, DateTime toDate)
    {
        // La lógica de la llamada se delega completamente a la fachada del otro contexto.
        // Este servicio actúa como un simple "pasamanos", pero aísla la dependencia.
        return await wateringContextFacade.FetchWateringLogsByDeviceIdAndDateRange(deviceId, fromDate, toDate);
    }
}