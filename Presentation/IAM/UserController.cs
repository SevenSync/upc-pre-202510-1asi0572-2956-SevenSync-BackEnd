using AutoMapper;
using Domain.IAM.Models.Aggregates;
using Domain.IAM.Models.Commands;
using Domain.IAM.Models.Queries;
using Domain.IAM.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.IAM.Response;
using Shared.Domain.IAM.Exceptions;

namespace Presentation.IAM;

[Route("api/users/")]
[ApiController]
public class UserController(
    IMapper mapper,
    IUserCommandService userCommandService,
    IUserQueryService userManagerQueryService
    ) : ControllerBase
{
    /// <summary>
    ///     Obtain the information of a user by its ID.
    /// </summary>
    /// <returns>
    ///     Returns the information of a user of type <c>UserInformation</c>.
    /// </returns>
    /// <remarks>
    ///     This endpoint returs the information of a user of type <c>UserInformation</c>.
    ///     <para>If you were expecting to return also the user credentials, well not obviously.</para>
    ///     <para>You only need to provide an Id to start searching.</para>
    ///     <para>Note that this controller must be set with the annotation <i>[Authorize]</i></para>
    ///     <para>so any request to get the information of any user (at least public information)</para>
    ///     <para>must be authorized through login. But for this purpose, this is set as public and</para>
    ///     <para>anyone can access to this controller without authorization.</para>
    /// </remarks>
    /// <response code="200">Returns <b>the information of the user</b>.</response>
    /// <response code="404">User <b>not found</b>.</response>
    /// <response code="500"><b>Something went wrong</b>. Have you tried to unplug the internet cable?</response>
    [HttpGet]
    [Route("findById")]
    [AllowAnonymous]
    public async Task<IActionResult> GetUserInformation([FromQuery] int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await userManagerQueryService.Handle(new GetUserByIdQuery(id));
        if (result == null)
        {
            throw new UserNotFoundException();
        }
        var response = mapper.Map<User, UserResponse>(result);
        return Ok(response);
    }
    
    /// <summary>
    ///     Obtain the information of a user by its username. Your input should match the username.
    /// </summary>
    /// <returns>
    ///     Returns the information of a user of type <c>UserInformation</c>.
    /// </returns>
    /// <remarks>
    ///     This endpoint returns the information of a user of type <c>UserInformation</c>.
    ///     <para>If you were expecting to return also the user credentials, well not obviously.</para>
    ///     <para>You only need to provide a username to start searching.</para>
    /// </remarks>
    /// <response code="200">Returns <b>the information of the user</b>.</response>
    /// <response code="404">User <b>not found</b>.</response>
    /// <response code="500"><b>Something went wrong</b>. Have you tried to unplug the internet cable?</response>
    [HttpGet]
    [Route("findByUsername")]
    [AllowAnonymous]
    public async Task<IActionResult> GetUserByEmail([FromQuery] GetUserByEmailQuery query)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var result = await userManagerQueryService.Handle(query);
        if (result == null)
        {
            throw new UserNotFoundException();
        }
        var response = mapper.Map<User, UserResponse>(result);
        return Ok(response);
    }
    
    [HttpPut]
    [Route("updateUser")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserCommand command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await userCommandService.Handle(command);

        return Ok();
    }
    
    /// <summary>
    ///     Register a new user into the system.
    /// </summary>
    /// <param name="command">Body request parameters that represents the basic information of a user.</param>
    /// <returns>
    ///     Returns a message confirming the new user registered.
    /// </returns>
    /// <remarks>
    ///     This endpoint allows the creation of new users providing basic information such as user information and credentials.
    ///     The parameter type of this endpoint is an instance of <c>UserRegistrationRequest</c>.
    ///     <para>Here is an overview of the parameters the makes <c>UserRegistrationRequest</c>: </para>
    ///         <para> &#149; <b>Username</b>: The username of the user. </para>
    ///         <para> &#149; <b>Password</b>: The password. </para>
    ///         <para> &#149; <b>Email</b>: The email of the user. </para>
    ///         <para> &#149; <b>PhoneNumber</b>: The phone number. </para>
    ///     <para>You may be wondering where are the two segments we focus on. Well, an account allows any user to</para>
    ///     <para>find, buy real states and also allows users to create publications. We wrapped them up in a single account</para>
    ///     <para>to develop easier account creations and management.</para>
    /// </remarks>
    /// <response code="200">Returns <b>a confirmation message</b> for the new user registered.</response>
    /// <response code="500"><b>Something wrong</b> appears to be with your query.</response>
    /// <response code="400">You <b>didn't provide correct information</b> for the creation of a new user.</response>
    [HttpPost]
    [Route("register")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationCommand command)
    {
        if (!this.ModelState.IsValid)
        {
            return BadRequest();
        }
        
        var generatedId = await userCommandService.Handle(command);
        if (generatedId > 0)
        {
            return StatusCode(
                StatusCodes.Status200OK,
                "User created successfully with id: " + generatedId
            );
        }
        return BadRequest("User has been generated with an invalid id... Something went wrong.");
    }
}