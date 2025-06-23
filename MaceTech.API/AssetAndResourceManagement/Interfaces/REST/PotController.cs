using System.Net.Mime;
using MaceTech.API.AssetAndResourceManagement.Domain.Model.Commands;
using MaceTech.API.AssetAndResourceManagement.Domain.Services;
using MaceTech.API.AssetAndResourceManagement.Interfaces.REST.Resources;
using MaceTech.API.AssetAndResourceManagement.Interfaces.REST.Responses;
using MaceTech.API.AssetAndResourceManagement.Interfaces.REST.Transform;
using MaceTech.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using MaceTech.API.IAM.Interfaces.ACL;
using MaceTech.API.Profiles.Interfaces.REST.Resources;
using Microsoft.AspNetCore.Mvc;

namespace MaceTech.API.AssetAndResourceManagement.Interfaces.REST;

[ApiController]
[Route("api/pots/")]
[Produces(MediaTypeNames.Application.Json)]
public class PotController(
    IPotQueryService potQueryService,
    IPotCommandService potCommandService,
    IIamContextFacade iamContextFacade
    ): ControllerBase
{
    [HttpPost("create")]
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
    [HttpPatch("assign")]
    public async Task<IActionResult> AssignPotToUser([FromBody] AssignPotToUserResource resource)
    {
        var uid = iamContextFacade.GetUserUidFromContext(this.HttpContext);
        var command = AssignPotToUserCommandFromResourceAssembler.ToCommandFromResource(uid, resource);
        var result = await potCommandService.Handle(command);
        if (result == null)
        {
            return BadRequest(new PotAssignedResponse(Success: false));
        }

        return Ok(new PotAssignedResponse(Success: true));
    }
    
    [HttpPut("update-metrics")]
    public async Task<IActionResult> UpdatePotInformation([FromBody] UpdatePotMetricsResource resource)
    {
        //  The pot must have an ID (potId) and some kind of key (string).
        //  Let's keep it simple for now~
        var command = UpdatePotMetricsCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await potCommandService.Handle(command);
        if (result == null)
        {
            return BadRequest(new PotMetricsUpdatedResponse(Success: false));
        }
        
        return Ok(new PotMetricsUpdatedResponse(Success: true));
    }
    
    [Authorize]
    [HttpGet("get/{potId:long}")]
    public async Task<IActionResult> GetPot(long potId)
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
    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllPots()
    {
        var uid = iamContextFacade.GetUserUidFromContext(this.HttpContext);
        var query = GetPotsByUserIdQueryFromResourceAssembler.ToQueryFromResource(uid);
        var result = await potQueryService.Handle(query);
        return Ok(result);
    }
    
    [HttpDelete("delete/{potId:long}")]
    public async Task<IActionResult> DeletePot(long potId)
    {
        //  Actually, just the stuff is allowed to delete pots.
        //  Let's consider it for later.
        
        var command = DeletePotCommandFromResourceAssembler.ToCommandFromResource(potId);
        var result = await potCommandService.Handle(command);
        if (result == null)
        {
            return BadRequest(new PotDeletedResponse(Success: false));
        }
        
        return Ok(new PotDeletedResponse(Success: true));
    }

    [Authorize]
    [HttpPatch("unassign/{potId:long}")]
    public async Task<IActionResult> UnassignPotFromUser(long potId)
    {
        return Ok();
    }
    
    [Authorize]
    [HttpPatch("link-plant")]
    public async Task<IActionResult> LinkPotToPlant()
    {
        return Ok();
    }
    
    [Authorize]
    [HttpPatch("unlink-plant")]
    public async Task<IActionResult> UnlinkPotFromPlant()
    {
        return Ok();
    }
}