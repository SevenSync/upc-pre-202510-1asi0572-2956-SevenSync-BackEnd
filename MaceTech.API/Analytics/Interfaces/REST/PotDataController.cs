using MaceTech.API.Analytics.Domain.Repositories;
using MaceTech.API.Analytics.Interfaces.REST.Resources;
using MaceTech.API.Analytics.Interfaces.REST.Transform;
using MaceTech.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace MaceTech.API.Analytics.Interfaces.REST;

[Authorize] // Protegeremos los endpoints
[ApiController]
[Route("api/analytics")]
public class PotDataController(IPotRecordCommandService potRecordCommandService) : ControllerBase
{
    [HttpPost("records")]
    [AllowAnonymous] // El dispositivo no se loguea, usa API Key (manejado por middleware)
    public async Task<IActionResult> CreatePotRecord([FromBody] CreatePotRecordResource resource)
    {
        var command = CreatePotRecordCommandFromResourceAssembler.ToCommandFromResource(resource);
        var record = await potRecordCommandService.Handle(command);
        // Devolvemos el ID como confirmación, en lugar de un simple mensaje.
        if (record == null)
        {
             return BadRequest(new { message = "Failed to create record." });
        }
        return CreatedAtAction(nameof(CreatePotRecord), new { id = record.Id }, new { message = "Record created successfully." });
    }
}