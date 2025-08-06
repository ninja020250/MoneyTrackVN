using System.Net;
using System.Text.Json;
using MoneyTrack.Application.Exceptions;

namespace MoneyTrack.Api.Middleware;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlerMiddleware>();
    }
}