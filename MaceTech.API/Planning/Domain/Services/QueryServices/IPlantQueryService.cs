using MaceTech.API.Planning.Domain.Model.Aggregates;
using MaceTech.API.Planning.Domain.Model.Queries;

namespace MaceTech.API.Planning.Domain.Services.QueryServices;

public interface IPlantQueryService
{
    Task<IEnumerable<Plant>> Handle(GetAllPlantsQuery query);
    
    Task<Plant?> Handle(GetPlantByIdQuery query);
}