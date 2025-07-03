using MaceTech.API.Planning.Domain.Model.Aggregates;
using MaceTech.API.Planning.Domain.Model.Queries;
using MaceTech.API.Planning.Domain.Repositories;
using MaceTech.API.Planning.Domain.Services.QueryServices;

namespace MaceTech.API.Planning.Application.Internal.QueryServices;

/// <summary>
/// Implementación del servicio de consulta para las plantas.
/// Orquesta la obtención de datos del catálogo de plantas.
/// </summary>
public class PlantQueryService(IPlantRepository plantRepository) : IPlantQueryService
{
    /// <summary>
    /// Maneja la consulta para obtener todas las plantas del catálogo.
    /// </summary>
    /// <param name="query">La consulta GetAllPlantsQuery.</param>
    /// <returns>Una colección de entidades Plant.</returns>
    public async Task<IEnumerable<Plant>> Handle(GetAllPlantsQuery query)
    {
        return await plantRepository.ListAsync();
    }
}