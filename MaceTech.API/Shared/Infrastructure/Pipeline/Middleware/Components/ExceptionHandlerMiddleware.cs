using MaceTech.API.IAM.Domain.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MaceTech.API.Shared.Infrastructure.Pipeline.Middleware.Components;

public class ExceptionHandlerMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(
        HttpContext context
        )
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }
    
    public async Task HandleExceptionAsync(
        HttpContext context, Exception exception
    )
    {
        var pd = exception switch
        {
            EmailAlreadyInUseException => new ProblemDetails
            {
                Type   = "EmailAlreadyInUseException",
                Title  = "That email is already registered",
                Status = StatusCodes.Status409Conflict,
                Detail = "Choose another email or sign in instead."
            },
            InvalidTokenException => new ProblemDetails()
            {
                Type   = "InvalidTokenException",
                Title  = "Provided token is invalid",
                Status = StatusCodes.Status401Unauthorized,
                Detail = "Maybe token has expired or is malformed. Please sign in again."
            },

            _ => new ProblemDetails
            {
                Type   = "FallThroughException",
                Title  = "Internal server error",
                Status = StatusCodes.Status409Conflict,
                Detail = "An unexpected error occurred. That's all we know."
            }
        };
        context.Response.ContentType = "application/problem+json";
        await context.Response.WriteAsJsonAsync(pd);
    }
}