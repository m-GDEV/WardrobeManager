using Microsoft.AspNetCore.Diagnostics;
using WardrobeManager.Api.Services.Interfaces;
using WardrobeManager.Shared.Models;
using WardrobeManager.Shared.Enums;


namespace WardrobeManager.Api;

internal sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    public async ValueTask<bool> TryHandleAsync(
            HttpContext context,
            Exception ex,
            CancellationToken cancellationToken)
    {
        // can't use scoped services in singleton
        using IServiceScope scope = _scopeFactory.CreateScope();
        var _loggingService = scope.ServiceProvider.GetRequiredService<ILoggingService>();

        // Log the error
        _logger.LogError(ex.Message);

        var log = new Log(ex.Message, ex.ToString(), LogType.UncaughtException);
        await _loggingService.CreateDatabaseAndConsoleLog(log);


        context.Response.StatusCode = 500;
        await context.Response.WriteAsync(ex.Message);
        
        // I think 'true' indicates we were able to handle the error
        return true;
    }
}

