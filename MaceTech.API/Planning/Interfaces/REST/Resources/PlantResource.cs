namespace MaceTech.API.Planning.Interfaces.REST.Resources;

public record PlantResource(
    int Id, 
    string CommonName, 
    string ScientificName, 
    string ImageUrl, 
    string Description
);