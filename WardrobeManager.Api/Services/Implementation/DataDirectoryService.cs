using WardrobeManager.Api.Database;
using WardrobeManager.Api.Services.Interfaces;
using WardrobeManager.Shared.Enums;
using WardrobeManager.Shared.Models;

namespace WardrobeManager.Api.Services.Implementation;

public class DataDirectoryService : IDataDirectoryService
{
    private readonly string _baseDirectory;

    // Likely unnecessary to put this in a service but whatever

    public DataDirectoryService(IConfiguration config, IWebHostEnvironment webHostEnvironment)
    {
        string DataDirectory = config["DataDirectory"] ?? throw new Exception("Data Directory: configuration value not set");

        _baseDirectory = webHostEnvironment.IsDevelopment() ? Path.GetFullPath(Path.Combine(webHostEnvironment.ContentRootPath, DataDirectory)) : DataDirectory;
    }

    public string GetBaseDataDirectory()
    {
        var path = _baseDirectory;
        Directory.CreateDirectory(path);
        return path;
    }

    public string GetDatabaseDirectory()
    {
        var path = Path.Combine(_baseDirectory, "db");
        Directory.CreateDirectory(path);
        return path;
    }

    public string GetImagesDirectory()
    {
        var path = Path.Combine(_baseDirectory, "images");
        Directory.CreateDirectory(path);
        return path;
    }
}