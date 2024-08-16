using Microsoft.AspNetCore.Diagnostics;


namespace WardrobeManager.Api;

internal sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
            HttpContext context,
            Exception exception,
            CancellationToken cancellationToken)
    {
        _logger.LogError(exception.Message);

        context.Response.StatusCode = 500;
        await context.Response.WriteAsync(exception.Message);

        return true;
    }
}

