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
    /// <summary>
    ///     Create a new pot.
    /// </summary>
    /// <remarks>
    ///     Manufacturers can create a new pot. An identifier is generated for the pot.
    /// </remarks>
    /// <response code="200">Returns the <b>identifier</b> of the created pot.</response>
    /// <response code="400">Couldn't create, sorry~</response>
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> CreatePot()
    {
        var result = await potCommandService.Handle(new CreatePotCommand());
        if (result == null)
        {
            return BadRequest();
        }

        return Ok(new PotCreatedResponse(result.Id));
    }
    
    /// <summary>
    ///     Get a pot.
    /// </summary>
    /// <remarks>
    ///     Get a pot by its identifier. 
    /// 
    ///     <para>Overview of all parameters:</para>
    ///         <para> &#149; <b>PotId</b>: The pot identifier. </para>
    /// </remarks>
    /// <response code="200">Returns the pot information.</response>
    /// <response code="401">Unauthorized. Check the token.</response>
    /// <response code="404">Couldn't find a pot.</response>
    [Authorize]
    [HttpGet("{potId:long}")]
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
    
    /// <summary>
    ///     Get all pots of the current user.
    /// </summary>
    /// <remarks>
    ///     This endpoint returns all pots of the current user. No need to pass any parameters.
    /// </remarks>
    /// <response code="200">Returns a <b>list</b> of all pots.</response>
    /// <response code="401">Unauthorized. Check the token.</response>
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetPots()
    {
        var uid = iamContextFacade.GetUserUidFromContext(this.HttpContext);
        var query = GetPotsByUserIdQueryFromResourceAssembler.ToQueryFromResource(uid);
        var result = await potQueryService.Handle(query);
        return Ok(result);
    }
    
    /// <summary>
    ///     Delete a pot.
    /// </summary>
    /// <remarks>
    ///     A pot can be <b>logically</b> deleted by its identifier.
    /// 
    ///     <para>Overview of all parameters:</para>
    ///         <para> &#149; <b>PotId</b>: The pot identifier. </para>
    /// </remarks>
    /// <response code="200">Returns a <b>confirmation</b> message.</response>
    /// <response code="400">Bad request: no pot found or couldn't delete.</response>
    /// <response code="401">Unauthorized. Check the token.</response>
    [HttpDelete("{potId:long}")]
    public async Task<IActionResult> DeletePot(long potId)
    {
        var command = DeletePotCommandFromResourceAssembler.ToCommandFromResource(potId);
        var result = await potCommandService.Handle(command);
        if (result == null)
        {
            return BadRequest(new PotDeletedResponse(Success: false));
        }
        
        return Ok(new PotDeletedResponse(Success: true));
    }
    
    /// <summary>
    ///     Assign a pot to a user.
    /// </summary>
    /// <remarks>
    ///     Assign a pot to a user by its identifier. Provide the pot new assignee information in the request body.
    /// 
    ///     <para>Overview of all parameters:</para>
    ///         <para> &#149; <b>PotId</b>: The pot identifier. </para>
    ///         <para> &#149; <b>Name</b>: A name for the pot. </para>
    ///         <para> &#149; <b>Location</b>: The exact location of the pot. </para>
    /// </remarks>
    /// <response code="200">Returns a <b>confirmation</b> message.</response>
    /// <response code="400">Bad request: check out your parameters.</response>
    /// <response code="401">Unauthorized. Check the token.</response>
    [Authorize]
    [HttpPut("{potId:long}/assignee")]
    public async Task<IActionResult> AssignPot(
        long potId,
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
    
    /// <summary>
    ///     Unassign a pot from a user.
    /// </summary>
    /// <remarks>
    ///     Unassign a pot from a user by its identifier. 
    /// 
    ///     <para>Overview of all parameters:</para>
    ///         <para> &#149; <b>PotId</b>: The pot identifier. </para>
    /// </remarks>
    /// <response code="200">Returns a <b>confirmation</b> message.</response>
    /// <response code="400">Bad request: check out your parameters.</response>
    /// <response code="401">Unauthorized. Check the token.</response>
    [Authorize]
    [HttpDelete("{potId:long}/assignee")]
    public async Task<IActionResult> UnassignPot(long potId)
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
    
    /// <summary>
    ///     Update a pot metrics.
    /// </summary>
    /// <remarks>
    ///     Update a pot metrics by its identifier. Provide the pot new metrics information in the request body.
    /// 
    ///     Overview of all parameters:
    /// 
    ///     &#149; <b>PotId</b>: The pot identifier.
    ///     
    ///     &#149; <b>BatteryLevel</b>: The battery level.
    ///     
    ///     &#149; <b>WaterLevel</b>: The water level.
    ///     
    ///     &#149; <b>Humidity</b>: The humidity value.
    ///     
    ///     &#149; <b>Luminance</b>: The luminance value.
    ///     
    ///     &#149; <b>Temperature</b>: The temperature value.
    ///     
    ///     &#149; <b>Ph</b>: The ph value.
    ///     
    ///     &#149; <b>Salinity</b>: The salinity value.
    /// </remarks>
    /// <response code="200">Returns a <b>confirmation</b> message.</response>
    /// <response code="400">Bad request: check out your parameters.</response>
    /// <response code="401">Unauthorized. Check the token.</response>
    [HttpPatch("{potId:long}/metrics")]
    [AllowAnonymous]
    public async Task<IActionResult> UpdateMetrics(
        long potId,
        [FromBody] UpdatePotMetricsResource resource
        )
    {
        var command = UpdatePotMetricsCommandFromResourceAssembler.ToCommandFromResource(potId, resource);
        var result = await potCommandService.Handle(command);
        if (result == null)
        {
            return BadRequest(new PotMetricsUpdatedResponse(Success: false));
        }
        
        return Ok(new PotMetricsUpdatedResponse(Success: true));
    }
    
    /// <summary>
    ///     Link a plant to a pot.
    /// </summary>
    /// <remarks>
    ///     Link a plant to a pot by its identifier. Provide the requested information.
    /// 
    ///     <para>Overview of all parameters:</para>
    ///         <para> &#149; <b>PotId</b>: The pot identifier. </para>
    ///         <para> &#149; <b>PlantId</b>: The plant identifier. </para>
    /// </remarks>
    /// <response code="200">Returns a <b>confirmation</b> message.</response>
    /// <response code="400">Bad request: check out your parameters.</response>
    /// <response code="401">Unauthorized. Check the token.</response>
    [Authorize]
    [HttpPut("{potId:long}/plant")]
    public async Task<IActionResult> LinkPlant(long potId, [FromBody] LinkPlantToPotResource resource)
    {
        var uid = iamContextFacade.GetUserUidFromContext(this.HttpContext);
        var command = LinkPlantToPotCommandFromResourceAssembler.ToCommandFromResource(potId, uid, resource);
        var result = await potCommandService.Handle(command);
        if (result == null)
        {
            return BadRequest(new LinkPlantResponse(Success: false));
        }

        return Ok(new LinkPlantResponse(Success: true));
    }
    
    /// <summary>
    ///     Unlink a plant to a pot.
    /// </summary>
    /// <remarks>
    ///     Unlink a plant to a pot by its identifier. Provide the requested information.
    /// 
    ///     <para>Overview of all parameters:</para>
    ///         <para> &#149; <b>PotId</b>: The pot identifier. </para>
    /// </remarks>
    /// <response code="200">Returns a <b>confirmation</b> message.</response>
    /// <response code="400">Bad request: check out your parameters.</response>
    /// <response code="401">Unauthorized. Check the token.</response>
    [Authorize]
    [HttpDelete("{potId:long}/plant")]
    public async Task<IActionResult> UnlinkPlant(long potId)
    {
        var uid = iamContextFacade.GetUserUidFromContext(this.HttpContext);
        var command = UnlinkPlantFromPotCommandResourceAssembler.ToCommandFromResource(potId, uid);
        var result = await potCommandService.Handle(command);
        
        if (result == null)
        {
            return BadRequest(new UnlinkPlantResponse(Success: false));
        }
        
        return Ok(new UnlinkPlantResponse(Success: true));
    }
}