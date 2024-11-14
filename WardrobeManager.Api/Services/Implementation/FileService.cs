using WardrobeManager.Api.Services.Interfaces;

namespace WardrobeManager.Api.Services.Implementation;

public class FileService : IFileService
{
    private readonly IDataDirectoryService _dataDirectoryService;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public FileService(IDataDirectoryService dataDirectoryService, IWebHostEnvironment webHostEnvironment)
    {
        _dataDirectoryService = dataDirectoryService;
        _webHostEnvironment = webHostEnvironment;
    }

    public string ParseGuid(Guid guid)
    {
        return guid.ToString().Replace("{", "").Replace("}", "");
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

        string path = Path.Combine(_dataDirectoryService.GetUploadsDirectory(), ParseGuid(properGuid));

        await File.WriteAllBytesAsync(path, imageBytes);
    }

    public async Task<byte[]> GetImage(string guid)
    {
        string path = Path.Combine(_dataDirectoryService.GetUploadsDirectory(), guid);
        string notFound = Path.Combine(_webHostEnvironment.WebRootPath, "images", "notfound.png");

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
