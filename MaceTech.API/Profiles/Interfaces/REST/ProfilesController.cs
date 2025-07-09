using System.Net.Mime;
using MaceTech.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using MaceTech.API.IAM.Interfaces.ACL;
using MaceTech.API.Profiles.Domain.Model.Queries;
using MaceTech.API.Profiles.Domain.Services;
using MaceTech.API.Profiles.Interfaces.REST.Resources;
using MaceTech.API.Profiles.Interfaces.REST.Responses;
using MaceTech.API.Profiles.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace MaceTech.API.Profiles.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]/")]
[Produces(MediaTypeNames.Application.Json)]
public class ProfilesController(
    IProfileCommandService profileCommandService, 
    IProfileQueryService profileQueryService,
    IIamContextFacade iamContextFacade
    ) : ControllerBase
{
    /// <summary>
    ///     Create a new profile for the current user.
    /// </summary>
    /// <remarks>
    ///     Create a new profile. This endpoint uses an instance of a <c>CreateProfileResource</c>.
    /// 
    ///     <para>Overview of all parameters:</para>
    ///         <para> &#149; <b>FirstName</b>: User's first name. </para>
    ///         <para> &#149; <b>LastName</b>: User's last name. </para>
    ///         <para> &#149; <b>Street</b>: Street address. </para>
    ///         <para> &#149; <b>BuildingNumber</b>: BuildingNumber. </para>
    ///         <para> &#149; <b>City</b>: A city. </para>
    ///         <para> &#149; <b>PostalCode</b>: City's postal code. </para>
    ///         <para> &#149; <b>Country</b>: The country. </para>
    ///         <para> &#149; <b>CountryCode</b>: The country's phone number code. </para>
    ///         <para> &#149; <b>PhoneNumber</b>: The phone number. </para>    
    /// </remarks>
    /// <response code="200">Returns a <b>confirmation</b> message.</response>
    /// <response code="400">Something went wrong, maybe check the parameters.</response>
    /// <response code="401">Unauthorized. Check the token.</response>
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(CreateProfileResource resource)
    {
        var uid = iamContextFacade.GetUserUidFromContext(this.HttpContext);
        if (string.IsNullOrWhiteSpace(uid))
        {
            return Unauthorized();
        }
        var createProfileCommand = CreateProfileCommandFromResourceAssembler.ToCommandFromResource(uid, resource);
        var profile = await profileCommandService.Handle(createProfileCommand);
        if (profile is null) return BadRequest();
        
        return Ok(new UserCreatedResponse(Created:true)); 
    }
    
    /// <summary>
    ///     Get the current logged-in user profile.
    /// </summary>
    /// <remarks>
    ///     Get <b>the</b> user profile. No parameters are required.
    /// </remarks>
    /// <response code="200">Returns: user identifier, full name, street address, and phone number.</response>
    /// <response code="400">User not found</response>
    /// <response code="401">Unauthorized. Check the token.</response>
    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var uid = iamContextFacade.GetUserUidFromContext(this.HttpContext);
        if (string.IsNullOrWhiteSpace(uid))
        {
            return Unauthorized();
        }
        
        var getProfileByUidQuery = new GetProfileByUidQuery(uid);
        var profile = await profileQueryService.Handle(getProfileByUidQuery);
        if (profile == null) return NotFound();
        
        var profileResource = ProfileResourceFromEntityAssembler.ToResourceFromEntity(profile);
        return Ok(profileResource);
    }
    
    /// <summary>
    ///     Check if the current user has a profile.
    /// </summary>
    /// <remarks>
    ///     Does the current user have a profile?
    /// </remarks>
    /// <response code="200">Returns a <b>HasProfile</b> boolean value.</response>
    /// <response code="401">Unauthorized. Check the token.</response>
    [Authorize]
    [HttpGet("me/existence")]
    public async Task<IActionResult> ExistsMe()
    {
        var uid = iamContextFacade.GetUserUidFromContext(this.HttpContext);
        if (string.IsNullOrWhiteSpace(uid))
        {
            return Unauthorized();
        }
        
        var hasProfileQuery = new HasProfileQuery(uid);
        var hasProfile = await profileQueryService.Handle(hasProfileQuery);
        return Ok(new HasProfileResponse(hasProfile));
    }
    
    /// <summary>
    ///     Update the current user profile.
    /// </summary>
    /// <remarks>
    ///     Update the current user profile. This endpoint uses an instance of a <c>UpdateProfileResource</c>.
    /// 
    ///     <para>Overview of all parameters:</para>
    ///         <para> &#149; <b>FirstName</b>: User's first name. </para>
    ///         <para> &#149; <b>LastName</b>: User's last name. </para>
    ///         <para> &#149; <b>Street</b>: Street address. </para>
    ///         <para> &#149; <b>BuildingNumber</b>: BuildingNumber. </para>
    ///         <para> &#149; <b>City</b>: A city. </para>
    ///         <para> &#149; <b>PostalCode</b>: City's postal code. </para>
    ///         <para> &#149; <b>Country</b>: The country. </para>
    ///         <para> &#149; <b>CountryCode</b>: The country's phone number code. </para>
    ///         <para> &#149; <b>PhoneNumber</b>: The phone number. </para>    
    /// </remarks>
    /// <response code="200">Returns a <b>confirmation</b> message.</response>
    /// <response code="400">Something went wrong, maybe check the parameters.</response>
    /// <response code="401">Unauthorized. Check the token.</response>
    [Authorize]
    [HttpPut("me")]
    public async Task<IActionResult> UpdateMe([FromBody] UpdateProfileResource resource)
    {
        var uid = iamContextFacade.GetUserUidFromContext(this.HttpContext);
        if (string.IsNullOrWhiteSpace(uid))
        {
            return Unauthorized();
        }
        
        var updateProfileCommand = UpdateProfileCommandFromResourceAssembler.ToCommandFromResource(uid, resource);
        
        await profileCommandService.Handle(updateProfileCommand);
        return Ok(new UserUpdatedResponse(Updated: true));
    }
}