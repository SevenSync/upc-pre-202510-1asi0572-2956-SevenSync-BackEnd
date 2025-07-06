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
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PasswordRecoveryResponse = MaceTech.API.IAM.Interfaces.REST.Responses.PasswordRecoveryResponse;

namespace MaceTech.API.IAM.Interfaces.REST;

[Authorize]
[ApiController]
[Route("api/v1/auth/")]
[Produces(MediaTypeNames.Application.Json)]
public class AuthController(
    IUserCommandService userCommandService,
    IUserQueryService userQueryService,
    FirebaseAuth auth,
    IEmailSender emailSender,
    IEmailComposer emailComposer,
    IOptions<FirebaseConfiguration> firebaseConfiguration
    ) : ControllerBase
{
    //  |: Variables
    private readonly FirebaseConfiguration _firebaseConfiguration = firebaseConfiguration.Value;
    private readonly HttpClient _httpClient = new HttpClient();
    
    //  |: Functions
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
    
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] SignUpResource resource)
    {
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

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] SignInResource resource)
    {
        var signIn = await this.SignInViaFirebase(resource.Email, resource.Password);
        if (signIn == null) return Unauthorized("Invalid credentials");
        
        var user = await userQueryService.Handle(new GetActiveUserByEmailQuery(resource.Email));
        if (user == null) return Unauthorized("User not found");
        
        var token = await userCommandService.Handle(new SignInCommand(resource.Email, resource.Password));
        return Ok(new { user.Uid, token.token });
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
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
    
    [HttpPatch("password-reset/request")]
    [AllowAnonymous]
    public async Task<IActionResult> RequestPasswordReset([FromBody] PasswordResetResource resource)
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
        await emailSender.SendEmailAsync(emailComposer.ComposePasswordResetEmail(resource.Email, link));

        return Ok(new PasswordRecoveryResponse(Sent: true));
    }
}   