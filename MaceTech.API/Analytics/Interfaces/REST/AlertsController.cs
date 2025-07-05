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
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> CreateAlert(string deviceId, [FromBody] CreateAlertResource resource)
    {
        var command = CreateAlertCommandFromResourceAssembler.ToCommandFromResource(resource, deviceId);
        var alert = await alertCommandService.Handle(command);
        var alertResource = AlertResourceFromEntityAssembler.ToResourceFromEntity(alert);
        
        // Devolvemos 201 Created con una referencia a dónde se podría consultar esta alerta en el futuro.
        return CreatedAtAction(nameof(GetAlertHistory), "Alerts", new { deviceId = alert.DeviceId, alertId = alert.Id }, alertResource);
    }

    // Dejamos el esqueleto para la HU "Obtener Historial de Alertas"
    [HttpGet]
    public Task<IActionResult> GetAlertHistory(string deviceId, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        // Esta lógica se implementará en la siguiente HU
        return Task.FromResult<IActionResult>(NotFound("Functionality not yet implemented."));
    }
}