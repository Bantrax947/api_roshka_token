using System.Net;
using System.Text.Json;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);

            if (httpContext.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
            {
                await HandleStatusResponseAsync(httpContext, "Necesitas un token de autenticación válido para acceder.");
            }
            else if (httpContext.Response.StatusCode == (int)HttpStatusCode.Forbidden)
            {
                await HandleStatusResponseAsync(httpContext, "No tienes permiso para acceder a este recurso.");
            }
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var errorDetails = new
        {
            StatusCode = context.Response.StatusCode,
            Message = "Ha ocurrido un error inesperado.",
            DetailedMessage = exception.Message
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(errorDetails));
    }

    private static Task HandleStatusResponseAsync(HttpContext context, string message)
    {
        context.Response.ContentType = "application/json";
        var errorDetails = new
        {
            StatusCode = context.Response.StatusCode,
            Message = message
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(errorDetails));
    }
}