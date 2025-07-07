namespace MaceTech.API.Planning.Interfaces.REST.Resources;

// Recurso principal que se devolverá al cliente
public record DevicePlantResource(
    int Id, 
    string DeviceId, 
    int PlantId, 
    string PlantCommonName
);