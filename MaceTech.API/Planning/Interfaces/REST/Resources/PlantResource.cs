namespace MaceTech.API.Planning.Interfaces.REST.Resources;

public record PlantResource(
    long Id, 
    string CommonName, 
    string ScientificName, 
    string ImageUrl, 
    string Description
);