using WardrobeManager.Shared.Models;
using WardrobeManager.Shared.Enums;

namespace WardrobeManager.Api.Database.Services.Interfaces;
public interface ILoggingService
{
    Task CreateDatabaseLog(Log log);
}
