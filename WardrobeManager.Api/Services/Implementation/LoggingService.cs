#region

using WardrobeManager.Api.Database.Entities;
using WardrobeManager.Api.Repositories;
using WardrobeManager.Api.Services.Interfaces;
using WardrobeManager.Shared.Enums;

#endregion

namespace WardrobeManager.Api.Services.Implementation;
public class LoggingService(IGenericRepository<Log> genericRepository, ILogger<LoggingService> logger)
    : ILoggingService
{
    public async Task CreateDatabaseAndConsoleLog(Log log)
    {
        await genericRepository.CreateAsync(log);
        if (log.Type is LogType.Error or LogType.UncaughtException)
        {
            logger.LogError("{title}, {description}", log.Title , log.Description);
        }
        else
        {
            logger.LogInformation("{title}, {description}", log.Title , log.Description);
        }

        await genericRepository.SaveAsync();
    }
}
