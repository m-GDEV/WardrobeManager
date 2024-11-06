using WardrobeManager.Api.Services.Interfaces;

namespace WardrobeManager.Api.Services.Implementation;

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public FileService(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public string ParseGuid(Guid guid)
    {
        return guid.ToString().Replace("{", "").Replace("}", "");
    }

    public string GetDefaultUploadPath()
    {
        string uploadsFolderPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

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
        string notFound = Path.Combine(_webHostEnvironment.WebRootPath, "images", "notfound.jpg");

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
