namespace MaceTech.API.Watering.Interfaces.REST.Resources;

public record WateringHistoryResource(
    string SmartPotId,
    IEnumerable<WateringLogResource> Historial,
    int TotalEjecuciones,
    double PromedioAguaMl
);