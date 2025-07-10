namespace MaceTech.API.Planning.Interfaces.REST.Resources;

// Recurso principal que se devolverá al cliente
public record DevicePlantResource(
    double MinTemp, 
    double MaxTemp, 
    double MinHumidity, 
    double MaxHumidity, 
    double MinLight, 
    double MaxLight, 
    double MinSalinity, 
    double MaxSalinity, 
    double MinPh, 
    double MaxPh
);