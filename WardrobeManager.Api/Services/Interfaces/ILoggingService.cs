#region

using WardrobeManager.Api.Database.Entities;

#endregion

namespace WardrobeManager.Api.Services.Interfaces;
public interface ILoggingService
{
    Task CreateDatabaseAndConsoleLog(Log log);
}
