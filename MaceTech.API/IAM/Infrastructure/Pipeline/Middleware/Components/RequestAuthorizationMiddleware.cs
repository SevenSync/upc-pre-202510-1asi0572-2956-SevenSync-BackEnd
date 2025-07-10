using MaceTech.API.IAM.Application.Internal.OutboundServices;
using MaceTech.API.IAM.Domain.Exceptions;
using MaceTech.API.IAM.Domain.Model.Queries;
using MaceTech.API.IAM.Domain.Services;
using MaceTech.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using System.Net;

namespace MaceTech.API.IAM.Infrastructure.Pipeline.Middleware.Components;

public class RequestAuthorizationMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(
        HttpContext context,
        IUserQueryService userQueryService,
        ITokenService tokenService
        )
    {
        var endpoint = context.GetEndpoint();

        if (endpoint == null || endpoint.Metadata.Any(m => m is AllowAnonymousAttribute))
        {
            await next(context);
            return;
        }

        try
        {

            var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
            if (token == null) throw new InvalidTokenException();

            var userId = await tokenService.ValidateToken(token);
            if (userId == null) throw new InvalidTokenException();

            var getUserByIdQuery = new GetUserByUidQuery(userId);
            var result = await userQueryService.Handle(getUserByIdQuery);
            if (result == null) throw new UserNotFoundException("User not found for the given token.");

            context.Items["User"] = result;
            await next(context);
        }
        catch (InvalidTokenException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.WriteAsync(ex.Message);
        }
        catch (UserNotFoundException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.WriteAsync(ex.Message);
        }
    }
}