namespace MaceTech.API.Planning.Interfaces.REST.Resources;

/// <summary>
/// Recurso que representa una planta del catálogo para ser consumido por los clientes de la API.
/// </summary>
public record PlantResource(
    int Id, 
    string CommonName, 
    string ScientificName, 
    string ImageUrl, 
    string Description
);