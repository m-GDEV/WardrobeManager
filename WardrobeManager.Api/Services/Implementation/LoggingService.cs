using WardrobeManager.Api.Database;
using WardrobeManager.Api.Services.Interfaces;
using WardrobeManager.Shared.Enums;
using WardrobeManager.Shared.Models;

namespace WardrobeManager.Api.Services.Implementation;
public class LoggingSerivce : ILoggingService
{
    private readonly DatabaseContext _context;
    private readonly ILogger<LoggingSerivce> _logger;

    public LoggingSerivce(DatabaseContext context, ILogger<LoggingSerivce> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task CreateDatabaseAndConsoleLog(Log log)
    {
        _context.Logs.Add(log);
        if (log.Type == LogType.Error || log.Type == LogType.UncaughtException)
        {
            _logger.LogError(log.Title + log.Description);
        }
        else
        {
            _logger.LogInformation(log.Title + log.Description);
        }
        await _context.SaveChangesAsync();
    }
}
