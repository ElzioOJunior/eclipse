using System.Diagnostics;
using Application.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Net;
using Newtonsoft.Json;

public class ErrorsHandling
{
    private readonly RequestDelegate _next;

    public ErrorsHandling(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context); 
        }
        catch (CustomException ex)
        {
            Debug.WriteLine($"CustomException intercepted: {ex.Message}");
            await HandleCustomExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Generic Exception intercepted: {ex.Message}");
            await HandleGenericExceptionAsync(context, ex);
        }
    }

    private static Task HandleCustomExceptionAsync(HttpContext context, CustomException exception)
    {
        var errorResponse = new
        {
            type = exception.Type,
            error = exception.Message,
            detail = exception.Detail
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        return context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
    }

    private static Task HandleGenericExceptionAsync(HttpContext context, Exception exception)
    {
        var errorResponse = new
        {
            type = "UnexpectedError",
            error = "An unexpected error occurred.",
            detail = exception.Message
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        return context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
    }
}
