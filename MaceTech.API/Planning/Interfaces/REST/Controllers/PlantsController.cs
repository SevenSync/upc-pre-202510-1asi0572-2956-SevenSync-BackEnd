using System.Net.Mime;
using MaceTech.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using MaceTech.API.Planning.Domain.Model.Queries;
using MaceTech.API.Planning.Domain.Services.QueryServices;
using MaceTech.API.Planning.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace MaceTech.API.Planning.Interfaces.REST.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class PlantsController(IPlantQueryService plantQueryService) : ControllerBase
{
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllPlants()
    {
        var query = new GetAllPlantsQuery();
        
        var plants = await plantQueryService.Handle(query);
        
        var resources = plants.Select(PlantResourceFromEntityAssembler.ToResourceFromEntity);
        
        return Ok(resources);
    }

    [Authorize]
    [HttpGet("{plantId:long}")]
    public async Task<IActionResult> GetPlantById(long plantId)
    {
        var query = new GetPlantByIdQuery(plantId);
        
        var plant = await plantQueryService.Handle(query);
        
        if (plant == null)
        {
            return NotFound(new { message = $"Plant with ID {plantId} not found." });
        }
        
        var resource = PlantResourceFromEntityAssembler.ToResourceFromEntity(plant);
        
        return Ok(resource);
    }
}