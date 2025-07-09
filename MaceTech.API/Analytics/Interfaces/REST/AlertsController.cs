using System.Net.Mime;
using MaceTech.API.Analytics.Domain.Services.CommandServices;
using MaceTech.API.Analytics.Interfaces.REST.Resources;
using MaceTech.API.Analytics.Interfaces.REST.Transform;
using MaceTech.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace MaceTech.API.Analytics.Interfaces.REST;

[Authorize]
[ApiController]
[Route("api/v1/alerts/device/{deviceId}")]
[Produces(MediaTypeNames.Application.Json)]
public class AlertsController(IAlertCommandService alertCommandService) : ControllerBase
{
    /// <summary>
    ///     Create an alert.
    /// </summary>
    /// <remarks>
    ///     Create an alert. Provide all requested parameters.
    /// 
    ///     Overview of all parameters:
    ///     
    ///     &#149; <b>DeviceId</b>: The device identifier.
    ///     
    ///     &#149; <b>AlertType</b>: The alert type.
    ///     
    ///     &#149; <b>Value</b>: The value.
    /// </remarks>
    /// <response code="200">Returns the <b>device identifier</b> and the <b>craeted alert identifier</b>.</response>
    /// <response code="401">Unauthorized. Check the token.</response>
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> CreateAlert(string deviceId, [FromBody] CreateAlertResource resource)
    {
        var command = CreateAlertCommandFromResourceAssembler.ToCommandFromResource(resource, deviceId);
        var alert = await alertCommandService.Handle(command);
        var alertResource = AlertResourceFromEntityAssembler.ToResourceFromEntity(alert);
        
        return CreatedAtAction(nameof(GetAlertHistory), "Alerts", new { deviceId = alert.DeviceId, alertId = alert.Id }, alertResource);
    }

    /// <summary>
    ///     Get alert history for a device.
    /// </summary>
    /// <remarks>
    ///     Get alert history for a device. Provide all requested parameters.
    /// 
    ///     Overview of all parameters:
    /// 
    ///     &#149; <b>DeviceId</b>: The device identifier.
    ///
    ///     &#149; <b>From</b>: From time.
    ///
    ///     &#149; <b>To</b>: to time.
    ///
    ///     Remember that <c>time</c> is given in the <b>ISO 8601</b> format, e.g. <c>2023-10-01T12:00:00Z</c>.
    /// </remarks>
    /// <response code="200">Returns a <b>confirmation</b> message.</response>
    /// <response code="400">The new password must be at least 6 characters long.</response>
    /// <response code="401">Unauthorized. Check the token.</response>
    [HttpGet]
    public Task<IActionResult> GetAlertHistory(string deviceId, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        return Task.FromResult<IActionResult>(NotFound("Functionality not yet implemented."));
    }
}