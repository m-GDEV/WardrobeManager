using WardrobeManager.Shared.Models;

namespace WardrobeManager.Api.Services.Interfaces;
public interface ILoggingService
{
    Task CreateDatabaseLog(Log log);
}
