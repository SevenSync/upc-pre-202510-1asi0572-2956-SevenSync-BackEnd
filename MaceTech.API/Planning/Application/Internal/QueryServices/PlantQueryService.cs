using MaceTech.API.Planning.Domain.Model.Aggregates;
using MaceTech.API.Planning.Domain.Model.Queries;
using MaceTech.API.Planning.Domain.Repositories;
using MaceTech.API.Planning.Domain.Services.QueryServices;

namespace MaceTech.API.Planning.Application.Internal.QueryServices;

public class PlantQueryService(IPlantRepository plantRepository) : IPlantQueryService
{
    public async Task<IEnumerable<Plant>> Handle(GetAllPlantsQuery query)
    {
        return await plantRepository.ListAsync();
    }
    public async Task<Plant?> Handle(GetPlantByIdQuery query)
    {
        return await plantRepository.FindByIdAsync(query.plantId);
    }
}