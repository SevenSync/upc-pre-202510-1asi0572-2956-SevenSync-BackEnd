using System.Net.Mime;
using FirebaseAdmin.Auth;
using MaceTech.API.IAM.Application.External.Email.Services;
using MaceTech.API.IAM.Domain.Exceptions;
using MaceTech.API.IAM.Domain.Model.Aggregates;
using MaceTech.API.IAM.Domain.Model.Commands;
using MaceTech.API.IAM.Domain.Model.Queries;
using MaceTech.API.IAM.Domain.Services;
using MaceTech.API.IAM.Infrastructure.Authentication.Firebase.Configuration;
using MaceTech.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using MaceTech.API.IAM.Interfaces.REST.Resources;
using MaceTech.API.IAM.Interfaces.REST.Responses;
using MaceTech.API.IAM.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PasswordRecoveryResponse = MaceTech.API.IAM.Interfaces.REST.Responses.PasswordRecoveryResponse;

namespace MaceTech.API.IAM.Interfaces.REST;

[Authorize]
[ApiController]
[Route("api/users/")]
[Produces(MediaTypeNames.Application.Json)]
public class AuthenticationController(
    IUserCommandService userCommandService,
    IUserQueryService userQueryService,
    FirebaseAuth auth,
    IEmailSender emailSender,
    IEmailComposer emailComposer,
    IOptions<FirebaseConfiguration> firebaseConfiguration
    ) : ControllerBase
{
    private readonly FirebaseConfiguration _firebaseConfiguration = firebaseConfiguration.Value;
    private readonly HttpClient _httpClient = new HttpClient();
    
    private async Task<FirebaseSignInResponse?> SignInViaFirebase(
        string email, string password, CancellationToken ct = default
    )
    {
        var apiKey = _firebaseConfiguration.WebApiKey;
        var signInUrl = $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={apiKey}";
        var body = JsonContent.Create(new
        {
            email, password, returnSercureToken = true
        });
        
        using var response = await this._httpClient.PostAsync(signInUrl, body, ct);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }
        
        return await response.Content.ReadFromJsonAsync<FirebaseSignInResponse>(cancellationToken: ct);
    }
    
    [HttpPost("sign-up")]
    [AllowAnonymous]
    public async Task<IActionResult> SingUp([FromBody] SignUpResource resource)
    {
        if (string.IsNullOrWhiteSpace(resource.Password) || resource.Password.Length < 6)
        {
            return BadRequest("Password must be at least 6 characters long.");
        }

        var args = new UserRecordArgs
        {
            Email = resource.Email,
            Password = resource.Password,
            DisplayName = resource.Email,
            EmailVerified = true,
            Disabled = false
        };

        try
        {
            var user = await auth.CreateUserAsync(args);
            
            var command = SignUpCommandFromResourceAssembler.ToCommandFromResource(user.Uid, resource);
            await userCommandService.Handle(command);
            
            var link = await auth.GenerateEmailVerificationLinkAsync(resource.Email);
            await emailSender.SendEmailAsync(emailComposer.ComposeWelcomeEmail(resource.Email, link));
            
            return Ok(new SignUpResponse(Created: true));
        }
        catch (FirebaseAuthException e) when (e.AuthErrorCode == AuthErrorCode.EmailAlreadyExists)
        {
            throw new EmailAlreadyInUseException(resource.Email);
        }
    }

    [HttpPost("sign-in")]
    [AllowAnonymous]
    public async Task<IActionResult> SignIn([FromBody] SignInResource resource)
    {
        var signIn = await this.SignInViaFirebase(resource.Email, resource.Password);
        if (signIn == null) return Unauthorized("Invalid credentials");
        
        var user = await userQueryService.Handle(new GetActiveUserByEmailQuery(resource.Email));
        if (user == null) return Unauthorized("User not found");
        
        var token = await userCommandService.Handle(new SignInCommand(resource.Email, resource.Password));
        return Ok(new { user.Uid, token.token });
    }

    [Authorize]
    [HttpPost("sign-out")]
    public async Task<IActionResult> SignOutUser()
    {
        if (
            !HttpContext.Items.TryGetValue("User", out var userDto) ||
            (userDto is not User user))
        {
            throw new InvalidTokenException();
        }
        
        var userId = user.Uid;
        await userCommandService.Handle(new SignOutCommand(userId));
        return NoContent();
    }
    
    [HttpPatch("password-recovery")]
    [AllowAnonymous]
    public async Task<IActionResult> PasswordRecovery([FromBody] PasswordRecoveryResource resource)
    {
        if (string.IsNullOrWhiteSpace(resource.Email))
        {
            return BadRequest("Email is required.");
        }

        var user = await userQueryService.Handle(new GetActiveUserByEmailQuery(resource.Email));
        if (user == null)
        {
            return NotFound("User not found.");
        }

        var link = await auth.GeneratePasswordResetLinkAsync(resource.Email);
        await emailSender.SendEmailAsync(emailComposer.ComposePasswordRecoveryEmail(resource.Email, link));

        return Ok(new PasswordRecoveryResponse(Sent: true));
    }
    
    [Authorize]
    [HttpPost("change-password")]
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
    [HttpDelete("delete-account")]
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