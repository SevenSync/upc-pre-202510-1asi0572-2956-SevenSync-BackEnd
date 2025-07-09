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
    /// <summary>
    ///     Get the user credentials.
    /// </summary>
    /// <remarks>
    ///     Get the user credential by the user id such as the email address.
    /// </remarks>
    /// <response code="200">Returns <b>the user id</b> and <b>the email address</b>.</response>
    /// <response code="404">No user found with that user id.</response>
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserById(string userId)
    {
        var getUserByIdQuery = new GetUserByUidQuery(userId);
        var user = await userQueryService.Handle(getUserByIdQuery);
        if (user is null) return NotFound();
        var userResource = UserResourceFromEntityAssembler.ToResourceFromEntity(user);
        return Ok(userResource);
    }
    
    /// <summary>
    ///     Change the password of the current user.
    /// </summary>
    /// <remarks>
    ///     Login into the system. This endpoint uses an instance of a <c>ChangePasswordResource</c>.
    /// 
    ///     <para>Overview of all parameters:</para>
    ///         <para> &#149; <b>NewPassword</b>: A new password. </para>
    /// </remarks>
    /// <response code="200">Returns a <b>confirmation</b> message.</response>
    /// <response code="400">The new password must be at least 6 characters long.</response>
    /// <response code="401">Unauthorized. Check the token.</response>
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

    /// <summary>
    ///     Delete an account.
    /// </summary>
    /// <remarks>
    ///     <b>Logically</b> delete an account. This endpoint does not require any parameters.
    /// </remarks>
    /// <response code="200">Returns a <b>confirmation</b> message.</response>
    /// <response code="401">Unauthorized. Check the token.</response>
    /// <response code="404">User not found.</response>
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