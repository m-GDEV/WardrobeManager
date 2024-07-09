using WardrobeManager.Api.Database.Services.Interfaces;
using WardrobeManager.Shared.Models;

namespace WardrobeManager.Api.Database.Services.Implementation;

public class FileService : IFileService
{
    public string ParseGuid(Guid guid)
    {
        return guid.ToString().Replace("{", "").Replace("}", "");
    }

    public string GetDefaultUploadPath()
    {
        // 1. Get the Project Root Directory (reliable approach):
        string projectRootPath = AppDomain.CurrentDomain.BaseDirectory;

        // 2. Construct the Full File Path:
        string uploadsFolderPath = Path.Combine(projectRootPath, "Uploads"); // "uploads" is the folder name 

        // 3. Create the folder if it doesn't exist:
        Directory.CreateDirectory(uploadsFolderPath); // sneaking it in so i don't have to later

        return uploadsFolderPath;
    }

    public async Task SaveImage(Guid? guid, string ImageBase64)
    {
        // we check on the client, but if the image is larger than our limit 
        // the base64 could be empty
        if (guid == null || ImageBase64 == string.Empty)
        {
            return; // silently exit
        }

        Guid properGuid = guid.GetValueOrDefault();

        byte[] imageBytes = Convert.FromBase64String(ImageBase64);

        string path = Path.Combine(GetDefaultUploadPath(), ParseGuid(properGuid));

        await File.WriteAllBytesAsync(path, imageBytes);
    }

    public async Task<byte[]> GetImage(string guid)
    {
        string path = Path.Combine(GetDefaultUploadPath(), guid);
        string notFound = Path.Combine(GetDefaultUploadPath(), "notfound.png");

        if (File.Exists(path))
        {
            byte[] imageBytes = await File.ReadAllBytesAsync(path);                                       // 6. Serve the file 
            return imageBytes;
        }

        else
        {
            byte[] imageBytes = await File.ReadAllBytesAsync(notFound);                                        // 6. Serve the file 
            return imageBytes;
        }
    }
}
