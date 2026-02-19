#region

using WardrobeManager.Api.Database.Entities;
using WardrobeManager.Api.Services.Interfaces;
using WardrobeManager.Shared.Enums;

#endregion

namespace WardrobeManager.Api.Middleware;

// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/write?view=aspnetcore-8.0
public class LoggingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var loggingService = context.RequestServices.GetRequiredService<ILoggingService>();

        var log = new Log("Request received", context.Connection.RemoteIpAddress?.ToString() ?? "No IP Address", LogType.RequestLog, LogOrigin.Backend);
        await loggingService.CreateDatabaseAndConsoleLog(log);

        await next(context);
    }
}