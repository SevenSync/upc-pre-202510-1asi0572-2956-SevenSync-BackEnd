using MaceTech.API.Planning.Domain.Model.Aggregates;
using MaceTech.API.Planning.Interfaces.REST.Resources;

namespace MaceTech.API.Planning.Interfaces.REST.Transform;

public static class PlantResourceFromEntityAssembler
{
    public static PlantResource ToResourceFromEntity(Plant entity)
    {
        return new PlantResource(
            entity.Id, 
            entity.CommonName, 
            entity.ScientificName, 
            entity.ImageUrl, 
            entity.Description
        );
    }
}