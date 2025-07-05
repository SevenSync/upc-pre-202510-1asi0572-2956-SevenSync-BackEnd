using MaceTech.API.Analytics.Domain.Services.CommandServices;
using MaceTech.API.Analytics.Interfaces.REST.Resources;
using MaceTech.API.Analytics.Interfaces.REST.Transform;
using MaceTech.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace MaceTech.API.Analytics.Interfaces.REST;

[Authorize]
[ApiController]
[Route("api/v1/analytics")]
public class PotDataController(IPotRecordCommandService potRecordCommandService) : ControllerBase
{
    [HttpPost("records")]
    [AllowAnonymous]
    public async Task<IActionResult> CreatePotRecord([FromBody] CreatePotRecordResource resource)
    {
        var command = CreatePotRecordCommandFromResourceAssembler.ToCommandFromResource(resource);
        var record = await potRecordCommandService.Handle(command);
        if (record == null)
        {
             return BadRequest(new { message = "Failed to create record." });
        }
        return CreatedAtAction(nameof(CreatePotRecord), new { id = record.Id }, new { message = "Record created successfully." });
    }
}