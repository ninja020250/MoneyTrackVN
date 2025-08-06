using System.Net;
using System.Text.Json;
using MoneyTrack.Application.Exceptions;

namespace MoneyTrack.Api.Middleware;

public class ExceptionHandlerMiddleware(RequestDelegate _next)
{
    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception e)
        {
            await ConvertException(httpContext, e);
        }
    }

    private Task ConvertException(HttpContext context, Exception exception)
    {
        HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;

        context.Response.ContentType = "application/json";

        var result = String.Empty;

        switch (exception)
        {
            case ValidationException validationException:
                httpStatusCode = HttpStatusCode.BadRequest;
                result = JsonSerializer.Serialize(validationException.ValidationErrors);
                break;
            case BadRequestException badRequestException:
                httpStatusCode = HttpStatusCode.BadRequest;
                result = badRequestException.Message;
                break;
            case NotFoundException notFoundException:
                httpStatusCode = HttpStatusCode.NotFound;
                break;
            case Exception:
                httpStatusCode = HttpStatusCode.BadRequest;
                break;
        }
        context.Response.StatusCode = (int)httpStatusCode;

        if (result == string.Empty)
        {
            result = JsonSerializer.Serialize(new { error = exception.Message });
        }
        
        return context.Response.WriteAsync(result);
    }
}