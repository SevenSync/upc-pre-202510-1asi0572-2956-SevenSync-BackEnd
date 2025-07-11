

using System.Net.Mime;
using MaceTech.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using MaceTech.API.Watering.Domain.Model.Queries;
using MaceTech.API.Watering.Domain.Services.CommandServices;
using MaceTech.API.Watering.Domain.Services.QueryServices;
using MaceTech.API.Watering.Interfaces.REST.Resources;
using MaceTech.API.Watering.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace MaceTech.API.Watering.Interfaces.REST;

[Authorize]
[ApiController]
[Route("api/v1/watering-history/device/{deviceId}/")]
[Produces(MediaTypeNames.Application.Json)]
public class WateringController(
    IWateringHistoryQueryService wateringHistoryQueryService,
    IWateringLogCommandService wateringLogCommandService 
) : ControllerBase
{
    [HttpPost("create-log")]
    [AllowAnonymous]
    public async Task<IActionResult> CreateWateringLog(long deviceId, [FromBody] CreateWateringLogResource resource)
    {
        var command = CreateWateringLogCommandFromResourceAssembler.ToCommandFromResource(resource, deviceId);
        var log = await wateringLogCommandService.Handle(command);

        if (log is null)   
        {
             return BadRequest(new { message = "Failed to create watering log." });
        }
        
        var logResource = WateringLogResourceFromEntityAssembler.ToResourceFromEntity(log);

        return Ok(logResource);
    }

    [HttpGet("from-data-range")]
    [AllowAnonymous]
    public async Task<IActionResult> GetWateringHistoryFromDataRange(long deviceId, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        var query = new GetWateringHistoryByDeviceIdAndDateRangeQuery(deviceId, from, to);
        
        var logs = await wateringHistoryQueryService.Handle(query);
        
        if (!logs.Historial.Any())
        {
            return NotFound(new { message = "No watering logs found for the specified device." });
        }
        
        var historyResource = WateringHistoryResourceFromEntityAssembler.ToResourceFromEntity(logs);
        
        return Ok(historyResource);
    }
    
    [Authorize]
    [HttpGet("all")]
    [AllowAnonymous]
    public async Task<IActionResult> GetWateringHistory(long deviceId)
    {
        var query = new GetWateringHistoryByDeviceIdQuery(deviceId);
        
        var logs = await wateringHistoryQueryService.Handle(query);
        
        if (!logs.Historial.Any())
        {
            return NotFound(new { message = "No watering logs found for the specified device." });
        }
        
        var historyResource = WateringHistoryResourceFromEntityAssembler.ToResourceFromEntity(logs);
        
        return Ok(historyResource);
    }
}