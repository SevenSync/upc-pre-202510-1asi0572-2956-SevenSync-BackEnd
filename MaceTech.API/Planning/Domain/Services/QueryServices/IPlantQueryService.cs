using MaceTech.API.Planning.Domain.Model.Aggregates;
using MaceTech.API.Planning.Domain.Model.Queries;

namespace MaceTech.API.Planning.Domain.Services.QueryServices;

/// <summary>
/// Contrato para el servicio que maneja las consultas relacionadas con las plantas.
/// </summary>
public interface IPlantQueryService
{
    /// <summary>
    /// Maneja la consulta para obtener todas las plantas.
    /// </summary>
    /// <param name="query">La consulta a ejecutar.</param>
    /// <returns>Una colección de entidades Plant.</returns>
    Task<IEnumerable<Plant>> Handle(GetAllPlantsQuery query);
}