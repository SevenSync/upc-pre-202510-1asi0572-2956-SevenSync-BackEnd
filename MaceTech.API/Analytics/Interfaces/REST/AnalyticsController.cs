using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using MaceTech.API.Analytics.Domain.Model.Queries;
using MaceTech.API.Analytics.Domain.Services.QueriesServices;
using MaceTech.API.Analytics.Interfaces.REST.Transform;
using MaceTech.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace MaceTech.API.Analytics.Interfaces.REST;

[Authorize]
[ApiController]
[Route("api/v1/analytics")]
[Produces(MediaTypeNames.Application.Json)]
public class AnalyticsController(IAnalyticsQueryService analyticsQueryService) : ControllerBase
{
    [HttpGet("comparison")]
    public async Task<IActionResult> GetPotComparison([Required][FromQuery] List<string> deviceIds)
    {
        if (deviceIds is null || deviceIds.Count == 0)
        {
            return BadRequest(new { message = "Please provide at least one device ID using the 'deviceIds' query parameter." });
        }

        var query = new GetPotComparisonQuery(deviceIds);
        var result = await analyticsQueryService.Handle(query);
        
        if (!result.Any())
        {
            return NotFound(new { message = "No data found for the provided device IDs in the last 7 days." });
        }

        var resource = result.Select(PotComparisonResourceFromValueObjectAssembler.ToResourceFromValueObject);
        return Ok(resource);
    }
}