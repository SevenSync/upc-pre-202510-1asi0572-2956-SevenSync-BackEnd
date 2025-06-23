using MaceTech.API.Shared.Infrastructure.Pipeline.Middleware.Components;

namespace MaceTech.API.Shared.Infrastructure.Pipeline.Middleware.Extensions;

public static class ExceptionHandlerExtensions
{
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlerMiddleware>();
    }
}