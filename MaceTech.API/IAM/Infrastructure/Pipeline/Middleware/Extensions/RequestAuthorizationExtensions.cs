using MaceTech.API.IAM.Infrastructure.Pipeline.Middleware.Components;

namespace MaceTech.API.IAM.Infrastructure.Pipeline.Middleware.Extensions;

public static class RequestAuthorizationExtensions
{
    public static IApplicationBuilder UseRequestAuthorization(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestAuthorizationMiddleware>();
    }
}