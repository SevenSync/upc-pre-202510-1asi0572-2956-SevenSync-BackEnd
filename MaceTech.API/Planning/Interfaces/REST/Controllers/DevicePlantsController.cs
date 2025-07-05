using System.Net.Mime;
using MaceTech.API.Planning.Domain.Model.Queries;
using MaceTech.API.Planning.Domain.Services.CommandServices;
using MaceTech.API.Planning.Domain.Services.QueryServices;
using MaceTech.API.Planning.Interfaces.REST.Resources;
using MaceTech.API.Planning.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaceTech.API.Planning.Interfaces.REST.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/devices/{deviceId}/plant-settings")]
[Produces(MediaTypeNames.Application.Json)]
public class DevicePlantsController(
    IDevicePlantCommandService devicePlantCommandService, 
    IDevicePlantQueryService devicePlantQueryService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AssignPlantToDevice(string deviceId, [FromBody] AssignPlantToDeviceResource resource)
    {
        var command = AssignPlantToDeviceCommandFromResourceAssembler.ToCommandFromResource(resource, deviceId);
        var result = await devicePlantCommandService.Handle(command);
        if (result == null) return BadRequest(new { message = "Plant not found or invalid operation." });
        
        var resultResource = DevicePlantResourceFromEntityAssembler.ToResourceFromEntity(result);
        return CreatedAtAction(nameof(GetPlantSettingsForDevice), new { deviceId = result.DeviceId }, resultResource);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetPlantSettingsForDevice(string deviceId)
    {
        var query = new GetPlantSettingsByDeviceIdQuery(deviceId);
        var result = await devicePlantQueryService.Handle(query);
        if (result == null) return NotFound(new { message = $"No plant settings found for device {deviceId}." });

        var resource = DevicePlantResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(resource);
    }
    
    [HttpPut("thresholds")]
    public async Task<IActionResult> UpdateDevicePlantThresholds(string deviceId, [FromBody] UpdateDevicePlantThresholdsResource resource)
    {
        var existingSettings = await devicePlantQueryService.Handle(new GetPlantSettingsByDeviceIdQuery(deviceId));
        if (existingSettings == null)
        {
            return NotFound(new { message = "No plant settings found for this device to update." });
        }
        
        var command = UpdateDevicePlantThresholdsCommandFromResourceAssembler.ToCommandFromResource(resource, existingSettings);
        
        try
        {
            var result = await devicePlantCommandService.Handle(command);
            var resultResource = DevicePlantResourceFromEntityAssembler.ToResourceFromEntity(result!);
            return Ok(resultResource);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}