using System.Net.Mime;
using MaceTech.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using MaceTech.API.Watering.Domain.Model.Queries;
using MaceTech.API.Watering.Domain.Services.QueryServices;
using MaceTech.API.Watering.Interfaces.REST.Resources;
using MaceTech.API.Watering.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace MaceTech.API.Watering.Interfaces.REST;

[Authorize]
[ApiController]
[Route("api/v1/watering/device/{deviceId}")]
[Produces(MediaTypeNames.Application.Json)]
public class WateringController(IWateringLogQueryService wateringLogQueryService) : ControllerBase
{
    [HttpGet("history")]
    public async Task<IActionResult> GetWateringHistory(string deviceId, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        var query = new GetWateringHistoryByDeviceIdQuery(deviceId, from, to);
        var logs = (await wateringLogQueryService.Handle(query)).ToList();

        if (!logs.Any())
        {
            return NotFound(new { error = "No watering records found for this device.", code = "NO_DATA" });
        }

        var logResources = logs.Select(WateringLogResourceFromEntityAssembler.ToResourceFromEntity);
        
        var historyResource = new WateringHistoryResource(
            SmartPotId: deviceId,
            Historial: logResources,
            TotalEjecuciones: logs.Count,
            PromedioAguaMl: logs.Average(l => l.WaterVolumeMl)
        );
        
        return Ok(historyResource);
    }
}