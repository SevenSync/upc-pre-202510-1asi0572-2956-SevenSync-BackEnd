using System.Net.Mime;
using MaceTech.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using MaceTech.API.Planning.Domain.Model.Queries;
using MaceTech.API.Planning.Domain.Services.CommandServices;
using MaceTech.API.Planning.Domain.Services.QueryServices;
using MaceTech.API.Planning.Interfaces.REST.Resources;
using MaceTech.API.Planning.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace MaceTech.API.Planning.Interfaces.REST.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/devices/")]
[Produces(MediaTypeNames.Application.Json)]
public class DevicePlantsController(
    IDevicePlantCommandService devicePlantCommandService, 
    IDevicePlantQueryService devicePlantQueryService) : ControllerBase
{
    [HttpPost("{deviceId}/assign-plant")]
    [AllowAnonymous]
    public async Task<IActionResult> AssignPlantToDevice(long deviceId, [FromBody] AssignPlantToDeviceResource resource)
    {
        var command = AssignPlantToDeviceCommandFromResourceAssembler.ToCommandFromResource(resource, deviceId);
        var result = await devicePlantCommandService.Handle(command);
        if (result == null) return BadRequest(new { message = "Plant not found or invalid operation." });
        
        var resultResource = DevicePlantResourceFromEntityAssembler.ToResourceFromEntity(result);
        return CreatedAtAction(nameof(GetPlantSettingsForDevice), new { deviceId = result.DeviceId }, resultResource);
    }
    
    [HttpGet("{deviceId:long}/get-plant-thresholds")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPlantSettingsForDevice(long deviceId)
    {
        var query = new GetPlantSettingsByDeviceIdQuery(deviceId);
        var result = await devicePlantQueryService.Handle(query);
        if (result == null) return NotFound(new { message = $"No plant thresholds found for device {deviceId}." });

        var resource = DevicePlantResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(resource);
    }
}