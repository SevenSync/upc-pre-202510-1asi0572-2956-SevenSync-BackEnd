using System.Net.Mime;
using FirebaseAdmin.Auth;
using MaceTech.API.IAM.Domain.Exceptions;
using MaceTech.API.IAM.Domain.Model.Aggregates;
using MaceTech.API.IAM.Domain.Model.Commands;
using MaceTech.API.IAM.Domain.Model.Queries;
using MaceTech.API.IAM.Domain.Services;
using MaceTech.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using MaceTech.API.IAM.Interfaces.REST.Resources;
using MaceTech.API.IAM.Interfaces.REST.Responses;
using MaceTech.API.IAM.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace MaceTech.API.IAM.Interfaces.REST;

[Authorize]
[ApiController]
[Route("api/v1/[controller]/")]
[Produces(MediaTypeNames.Application.Json)]
public class UsersController(
    IUserCommandService userCommandService,
    IUserQueryService userQueryService,
    FirebaseAuth auth
    ) : ControllerBase
{
    [HttpGet("{uid}")]
    public async Task<IActionResult> GetUserById(string uid)
    {
        var getUserByIdQuery = new GetUserByUidQuery(uid);
        var user = await userQueryService.Handle(getUserByIdQuery);
        if (user is null) return NotFound();
        var userResource = UserResourceFromEntityAssembler.ToResourceFromEntity(user);
        return Ok(userResource);
    }
    
    [Authorize]
    [HttpPost("me/password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordResource resource)
    {
        if (string.IsNullOrWhiteSpace(resource.NewPassword) || resource.NewPassword.Length < 6)
        {
            return BadRequest("New password must be at least 6 characters long.");
        }

        if (
            !HttpContext.Items.TryGetValue("User", out var userDto) ||
            (userDto is not User user))
        {
            throw new InvalidTokenException();
        }
        
        var args = new UserRecordArgs
        {
            Uid = user.Uid,
            Password = resource.NewPassword
        };

        try
        {
            await auth.UpdateUserAsync(args);
            return Ok(new ChangePasswordResponse(Success: true));
        }
        catch (FirebaseAuthException e) when (e.AuthErrorCode == AuthErrorCode.UserNotFound)
        {
            return NotFound("User not found.");
        }
    }

    [Authorize]
    [HttpDelete("me")]
    public async Task<IActionResult> DeleteAccount()
    {
        if (
            !HttpContext.Items.TryGetValue("User", out var userDto) ||
            (userDto is not User user))
        {
            throw new InvalidTokenException();
        }

        try
        {
            await auth.DeleteUserAsync(user.Uid);
            await userCommandService.Handle(new DeleteUserCommand(user.Uid));
            return Ok(new DeleteUserResponse(Deleted: true));
        }
        catch (FirebaseAuthException e) when (e.AuthErrorCode == AuthErrorCode.UserNotFound)
        {
            return NotFound("User not found.");
        }
    }
}