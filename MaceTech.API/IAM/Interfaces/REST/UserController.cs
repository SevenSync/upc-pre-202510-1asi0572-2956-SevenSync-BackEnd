using System.Net.Mime;
using MaceTech.API.IAM.Domain.Model.Queries;
using MaceTech.API.IAM.Domain.Services;
using MaceTech.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using MaceTech.API.IAM.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace MaceTech.API.IAM.Interfaces.REST;

[Authorize]
[ApiController]
[Route("api/v1/[controller]/")]
[Produces(MediaTypeNames.Application.Json)]
public class UserController(IUserQueryService userQueryService) : ControllerBase
{
    [HttpGet("{uid}")]
    public async Task<IActionResult> GetUser(string uid)
    {
        var getUserByIdQuery = new GetUserByUidQuery(uid);
        var user = await userQueryService.Handle(getUserByIdQuery);
        if (user is null) return NotFound();
        var userResource = UserResourceFromEntityAssembler.ToResourceFromEntity(user);
        return Ok(userResource);
    }
}