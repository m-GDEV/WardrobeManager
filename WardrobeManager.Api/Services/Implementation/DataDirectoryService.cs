#region

using WardrobeManager.Api.Services.Interfaces;

#endregion

namespace WardrobeManager.Api.Services.Implementation;

public class DataDirectoryService : IDataDirectoryService
{
    private readonly string _baseDirectory;

    // Likely unnecessary to put this in a service but whatever

    public DataDirectoryService(IConfiguration config, IWebHostEnvironment webHostEnvironment)
    {
        string DataDirectory = config["WM_DATA_DIRECTORY"] ?? throw new Exception("Data Directory: configuration value not set");

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
    public string GetUploadsDirectory()
    {
        var path = Path.Combine(GetImagesDirectory(), "uploads");
        Directory.CreateDirectory(path);
        return path;
    }
    public string GetDeletedUploadsDirectory()
    {
        var path = Path.Combine(GetImagesDirectory(), "deleted");
        Directory.CreateDirectory(path);
        return path;
    }
}