using System.Net.Mime;
using MaceTech.API.ARM.Domain.Model.Commands;
using MaceTech.API.ARM.Domain.Services;
using MaceTech.API.ARM.Interfaces.REST.Resources;
using MaceTech.API.ARM.Interfaces.REST.Responses;
using MaceTech.API.ARM.Interfaces.REST.Transform;
using MaceTech.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using MaceTech.API.IAM.Interfaces.ACL;
using MaceTech.API.Profiles.Interfaces.REST.Resources;
using Microsoft.AspNetCore.Mvc;

namespace MaceTech.API.ARM.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]/")]
[Produces(MediaTypeNames.Application.Json)]
public class PotsController(
    IPotQueryService potQueryService,
    IPotCommandService potCommandService,
    IIamContextFacade iamContextFacade
    ): ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreatePot([FromBody] CreatePotResource resource)
    {
        var result = await potCommandService.Handle(new CreatePotCommand());
        if (result == null)
        {
            return BadRequest(new PotCreatedResponse(Success: false));
        }

        return Ok(new PotCreatedResponse(Success: true));
    }
    
    [Authorize]
    [HttpGet("{potId:long}")]
    public async Task<IActionResult> GetPot([FromQuery] long potId)
    {
        iamContextFacade.GetUserUidFromContext(this.HttpContext);
        var query = GetPotQueryFromResponseAssembler.ToQueryFromResource(potId);
        var pot = await potQueryService.Handle(query);
        if (pot == null)
        {
            return NotFound();
        }

        return Ok(pot);
    }
    
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetPots()
    {
        var uid = iamContextFacade.GetUserUidFromContext(this.HttpContext);
        var query = GetPotsByUserIdQueryFromResourceAssembler.ToQueryFromResource(uid);
        var result = await potQueryService.Handle(query);
        return Ok(result);
    }
    
    [HttpDelete("{potId:long}")]
    public async Task<IActionResult> DeletePot([FromQuery] long potId)
    {
        var command = DeletePotCommandFromResourceAssembler.ToCommandFromResource(potId);
        var result = await potCommandService.Handle(command);
        if (result == null)
        {
            return BadRequest(new PotDeletedResponse(Success: false));
        }
        
        return Ok(new PotDeletedResponse(Success: true));
    }
    
    [Authorize]
    [HttpPut("{potId:long}/assignee")]
    public async Task<IActionResult> AssignPot(
        [FromQuery] long potId,
        [FromBody] AssignPotToUserResource resource
        )
    {
        var uid = iamContextFacade.GetUserUidFromContext(this.HttpContext);
        var command = AssignPotToUserCommandFromResourceAssembler.ToCommandFromResource(potId, uid, resource);
        var result = await potCommandService.Handle(command);
        if (result == null)
        {
            return BadRequest(new PotAssignedResponse(Success: false));
        }

        return Ok(new PotAssignedResponse(Success: true));
    }
    
    [Authorize]
    [HttpDelete("{potId:long}/assignee")]
    public async Task<IActionResult> UnassignPot([FromQuery] long potId)
    {
        var uid = iamContextFacade.GetUserUidFromContext(this.HttpContext);
        var command = UnassignPotFromUserCommandFromResourceAssembler.ToCommandFromResource(potId, uid);
        var result = await potCommandService.Handle(command);
        if (result == null)
        {
            return BadRequest(new PotUnassignedResponse(Success: false));
        }
        
        return Ok(new PotUnassignedResponse(Success: true));
    }
    
    [HttpPatch("{potId:long}/metrics")]
    public async Task<IActionResult> UpdateMetrics(
        [FromQuery] long potId,
        [FromBody] UpdatePotMetricsResource resource
        )
    {
        //  The pot must have an ID (potId) and some kind of key (string).
        //  Let's keep it simple for now~
        var command = UpdatePotMetricsCommandFromResourceAssembler.ToCommandFromResource(potId, resource);
        var result = await potCommandService.Handle(command);
        if (result == null)
        {
            return BadRequest(new PotMetricsUpdatedResponse(Success: false));
        }
        
        return Ok(new PotMetricsUpdatedResponse(Success: true));
    }
    
    [Authorize]
    [HttpPut("{potId:long}/plant")]
    public async Task<IActionResult> LinkPlant([FromQuery] long potId)
    {
        //  var uid = iamContextFacade.GetUserUidFromContext(this.HttpContext);
        //  var command = LinkPlantToPotCommandFromResourceAssembler.ToCommandFromResource(potId, uid);
        
        //  var uid = iamContextFacade.GetUserUidFromContext(this.HttpContext);
        //  var command = UnassignPotFromUserCommandFromResourceAssembler.ToCommandFromResource(potId, uid);
        //  var result = await potCommandService.Handle(command);
        //  if (result == null)
        //  {
        //      return BadRequest(new PotUnassignedResponse(Success: false));
        //  }
        //  
        //  return Ok(new PotUnassignedResponse(Success: true));

        return Ok();
    }
    
    [Authorize]
    [HttpDelete("{potId:long}/plant")]
    public async Task<IActionResult> UnlinkPlant(long potId)
    {
        // to be implemented
        return Ok();
    }
}