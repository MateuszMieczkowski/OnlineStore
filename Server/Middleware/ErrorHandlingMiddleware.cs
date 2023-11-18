using OnlineStore.Server.Services.Exceptions;
using System.Text.Json;

namespace OnlineStore.Server.Middleware;

public class ErrorHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (BadRequestException badRequestException)
        {
            await HandleExceptionAsync(context, badRequestException, 400);
        }
        catch (NotFoundException notFoundException)
        {
            await HandleExceptionAsync(context, notFoundException, 404);
        }
        catch (DuplicateException duplicateException)
        {
            await HandleExceptionAsync(context, duplicateException, 409);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);

            await HandleExceptionAsync(context, e, 500);
        }
    }
    
    private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception, int statusCode)
    {
        var response = new
        {
            exceptionType = GetExceptionType(exception),
            statusCode,
            message = exception.Message,
        };

        httpContext.Response.ContentType = "application/json";

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private static string GetExceptionType(Exception exception) => exception.GetType().Name;
}