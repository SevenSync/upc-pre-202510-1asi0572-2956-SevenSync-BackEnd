using System.Net.Mime;
using MaceTech.API.Planning.Domain.Model.Queries;
using MaceTech.API.Planning.Domain.Services.QueryServices;
using MaceTech.API.Planning.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaceTech.API.Planning.Interfaces.REST.Controllers;

[Authorize] // Requiere que el usuario esté autenticado para acceder a este controlador.
[ApiController]
[Route("api/v1/[controller]")] // La ruta será /api/v1/plants
[Produces(MediaTypeNames.Application.Json)]
public class PlantsController(IPlantQueryService plantQueryService) : ControllerBase
{
    /// <summary>
    /// Obtiene el catálogo completo de plantas disponibles.
    /// </summary>
    /// <returns>Una lista de todas las plantas del sistema.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllPlants()
    {
        // 1. Crear la Query
        var query = new GetAllPlantsQuery();
        
        // 2. Delegar la ejecución al servicio de aplicación
        var plants = await plantQueryService.Handle(query);
        
        // 3. Transformar las entidades de dominio a recursos de la API
        var resources = plants.Select(PlantResourceFromEntityAssembler.ToResourceFromEntity);
        
        // 4. Devolver la respuesta
        return Ok(resources);
    }
}