using WardrobeManager.Api.Database;
using WardrobeManager.Api.Services.Interfaces;
using WardrobeManager.Shared.Models;

namespace WardrobeManager.Api.Services.Implementation;
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
