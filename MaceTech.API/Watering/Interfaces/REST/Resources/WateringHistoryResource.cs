namespace MaceTech.API.Watering.Interfaces.REST.Resources;

public record WateringHistoryResource(
    long Id,
    long DecideId,
    IEnumerable<WateringLogResource> Historial,
    int TotalEjecuciones,
    double PromedioAguaMl
);