using System.Net.Mime;
using MaceTech.API.Planning.Domain.Model.Queries;
using MaceTech.API.Planning.Domain.Services.QueryServices;
using MaceTech.API.Planning.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaceTech.API.Planning.Interfaces.REST.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class PlantsController(IPlantQueryService plantQueryService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllPlants()
    {
        var query = new GetAllPlantsQuery();
        
        var plants = await plantQueryService.Handle(query);
        
        var resources = plants.Select(PlantResourceFromEntityAssembler.ToResourceFromEntity);
        
        return Ok(resources);
    }
}