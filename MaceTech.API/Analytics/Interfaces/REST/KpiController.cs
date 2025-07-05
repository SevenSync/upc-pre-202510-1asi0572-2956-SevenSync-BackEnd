using System.Net.Mime;
using MaceTech.API.Analytics.Domain.Model.Queries;
using MaceTech.API.Analytics.Domain.Services.QueriesServices;
using MaceTech.API.Analytics.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaceTech.API.Analytics.Interfaces.REST;

[Authorize]
[ApiController]
[Route("api/v1/kpis")]
[Produces(MediaTypeNames.Application.Json)]
public class KpiController(IKpiQueryService kpiQueryService) : ControllerBase
{
    [HttpGet("water-saved")]
    public async Task<IActionResult> GetWaterSavedKpi([FromQuery] string deviceId, [FromQuery] DateTime date)
    {
        if (string.IsNullOrWhiteSpace(deviceId))
        {
            return BadRequest(new { message = "El 'deviceId' es requerido." });
        }

        var query = new GetWaterSavedKpiQuery(deviceId, date);
        var kpi = await kpiQueryService.Handle(query);
        var resource = WaterSavedKpiResourceFromValueObjectAssembler.ToResourceFromValueObject(kpi);

        return Ok(resource);
    }
}