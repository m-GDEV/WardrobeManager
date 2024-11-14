using WardrobeManager.Shared.Models;

namespace WardrobeManager.Api.Services.Interfaces;

public interface IDataDirectoryService
{
    public string GetBaseDataDirectory();
    public string GetDatabaseDirectory();
    public string GetImagesDirectory();
    public string GetUploadsDirectory();
}
