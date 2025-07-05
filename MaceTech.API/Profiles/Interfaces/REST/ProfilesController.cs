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
[Route("api/v1/profiles/")]
[Produces(MediaTypeNames.Application.Json)]
public class ProfilesController(
    IProfileCommandService profileCommandService, 
    IProfileQueryService profileQueryService,
    IIamContextFacade iamContextFacade
    ) : ControllerBase
{
    [Authorize]
    [HttpPost("create")]
    public async Task<IActionResult> CreateProfile(CreateProfileResource resource)
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
    
    [Authorize]
    [HttpGet("get")]
    public async Task<IActionResult> GetProfileByUid()
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
    
    [Authorize]
    [HttpGet("has-profile")]
    public async Task<IActionResult> HasProfile()
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
    
    [Authorize]
    [HttpPut("update")]
    public async Task<IActionResult> UpdateProfile(UpdateProfileResource resource)
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