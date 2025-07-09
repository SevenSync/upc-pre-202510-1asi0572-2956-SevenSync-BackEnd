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
    
    /// <summary>
    ///     Register a new user.
    /// </summary>
    /// <param name="resource">Representation of a registration request. Fill up all parameters.</param>
    /// <remarks>
    ///     Register into the system. This endpoint uses an instance of a <c>SignUpResource</c>.
    /// 
    ///     <para>Overview of all parameters:</para>
    ///         <para> &#149; <b>Email</b>: A functional email. </para>
    ///         <para> &#149; <b>Password</b>: The password. </para>
    ///
    ///     After registration, remember to verify your email address by clicking the link sent to your email.
    /// </remarks>
    /// <response code="200">Returns <b>a confirmation message</b> for the new user registered.</response>
    /// <response code="400">You <b>didn't provide correct information</b> or the email is <b>already taken</b>.</response>
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

    /// <summary>
    ///     Login a user.
    /// </summary>
    /// <param name="resource">Representation of a login request. Fill up all parameters.</param>
    /// <remarks>
    ///     Login into the system. This endpoint uses an instance of a <c>SignInResource</c>.
    /// 
    ///     <para>Overview of all parameters:</para>
    ///         <para> &#149; <b>Email</b>: A registered and verified email. </para>
    ///         <para> &#149; <b>Password</b>: The password. </para>
    /// </remarks>
    /// <response code="200">Returns <b>the associated user identifier</b> and a <b>token</b>.</response>
    /// <response code="401">Unauthorized. What else to be said.</response>
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

    /// <summary>
    ///     Logout a user.
    /// </summary>
    /// <remarks>
    ///     Logout. Just as simple as that.
    /// </remarks>
    /// <response code="200">No returns. An ok response shows success.</response>
    /// <response code="401">Unauthorized. Check the token; maybe it has expired.</response>
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
    
    /// <summary>
    ///     A password reset request.
    /// </summary>
    /// <param name="resource">Representation of a password-reset request resource. Fill up all parameters.</param>
    /// <remarks>
    ///     Request a password reset. This endpoint uses an instance of a <c>PasswordResetResource</c>.
    /// 
    ///     <para>Overview of all parameters:</para>
    ///         <para> &#149; <b>Email</b>: A registered and verified email. </para>
    ///
    ///     Check your email inbox for a link to reset your password and follow through.
    /// </remarks>
    /// <response code="200">Returns <b>a confirmation message</b>.</response>
    /// <response code="400">In invalid email has been used.</response>
    /// <response code="404">No user found.</response>
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