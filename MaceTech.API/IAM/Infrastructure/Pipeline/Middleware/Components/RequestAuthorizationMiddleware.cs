using MaceTech.API.IAM.Application.Internal.OutboundServices;
using MaceTech.API.IAM.Domain.Model.Queries;
using MaceTech.API.IAM.Domain.Services;
using MaceTech.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;

namespace MaceTech.API.IAM.Infrastructure.Pipeline.Middleware.Components;

public class RequestAuthorizationMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(
        HttpContext context,
        IUserQueryService userQueryService,
        ITokenService tokenService
        )
    {
        var allowAnonymous = context.Request.HttpContext.GetEndpoint()!.Metadata
            .Any(m => m.GetType() == typeof(AllowAnonymousAttribute));
        if (allowAnonymous)
        {
            await next(context);
            return;
        }

        var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
        if (token == null) throw new Exception("Null or invalid token");
        var userId = await tokenService.ValidateToken(token);
        if (userId == null) throw new Exception("Invalid token");
        var getUserByIdQuery = new GetUserByIdQuery(userId.Value);
        var result = await userQueryService.Handle(getUserByIdQuery);
        if (result == null) throw new Exception("User not found");
        
        context.Items["User"] = result;
        await next(context);
    }
}