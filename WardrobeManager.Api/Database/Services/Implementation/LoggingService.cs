using WardrobeManager.Shared.Models;
using WardrobeManager.Shared.Enums;
using WardrobeManager.Api.Database.Services.Interfaces;
using SQLitePCL;

namespace WardrobeManager.Api.Database.Services.Implementation;
public class LoggingSerivce : ILoggingService
{
    private DatabaseContext _context { get; set; } 

    public LoggingSerivce(DatabaseContext context)
    {
        _context = context;
    }

    public async Task CreateDatabaseLog(Log log)
    {

    }
}
