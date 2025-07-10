using MaceTech.API.Watering.Domain.Model.Aggregates;
using MaceTech.API.Watering.Domain.Model.Commands;
using MaceTech.API.Watering.Interfaces.REST.Resources;
        
namespace MaceTech.API.Watering.Domain.Model.Entity;
        
public class WateringHistory
{
    public long Id { get; }
    public long deviceId { get; private set; }
    public IEnumerable<WateringLog> Historial { get; private set; }
    public int TotalEjecuciones { get; private set; }
    public double PromedioAguaMl { get; private set; }
        
    public WateringHistory(
        long deviceId,
        IEnumerable<WateringLog> historial)
    {
        this.deviceId = deviceId;
        var historialList = historial.ToList();
        Historial = historialList;
        TotalEjecuciones = historialList.Count;
        PromedioAguaMl = historialList.Any()
            ? historialList.Average(log => log.WaterVolumeMl)
            : 0.0;
    }
        
    public void OrderByTimestamp()
    {
        Historial = Historial.OrderByDescending(log => log.Timestamp);
    }
}