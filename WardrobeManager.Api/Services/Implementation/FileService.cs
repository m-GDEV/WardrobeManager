#region

using SQLitePCL;
using WardrobeManager.Api.Services.Interfaces;

#endregion

namespace WardrobeManager.Api.Services.Implementation;

public class FileService(
    IDataDirectoryService dataDirectoryService,
    IWebHostEnvironment webHostEnvironment,
    ILogger<FileService> logger,
    IConfiguration configuration)
    : IFileService
{
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

        // 5MB default max file size
        var maxFileSize = configuration["WM_MAX_IMAGE_UPLOAD_SIZE_IN_MB"] ?? "5";
        int maxFileSizeNum = Convert.ToInt32(maxFileSize);
        maxFileSizeNum *= 1024 * 1024;

        if (imageBytes.Length > maxFileSizeNum)
        {
            throw new Exception(
                $"File size too large! Received file size: {imageBytes.Length / 1024} MB. Max file size: {maxFileSizeNum / 1024} MB");
        }

        string path = Path.Combine(dataDirectoryService.GetUploadsDirectory(), ParseGuid(properGuid));

        await File.WriteAllBytesAsync(path, imageBytes);
    }

    public async Task<byte[]> GetImage(string guid)
    {
        string path = Path.Combine(dataDirectoryService.GetUploadsDirectory(), guid);
        string notFound = Path.Combine(webHostEnvironment.WebRootPath, "images", "notfound.jpg");

        if (File.Exists(path))
        {
            byte[] imageBytes = await File.ReadAllBytesAsync(path); // 6. Serve the file
            return imageBytes;
        }

        else
        {
            byte[] imageBytes = await File.ReadAllBytesAsync(notFound); // 6. Serve the file
            return imageBytes;
        }
    }

    public async Task DeleteImage(Guid givenGuid)
    {
        var guid = ParseGuid(givenGuid);
        string path = Path.Combine(dataDirectoryService.GetUploadsDirectory(), guid);
        string deletePath = Path.Combine(dataDirectoryService.GetDeletedUploadsDirectory(), guid);
    
        // Move deleted images to deleted folder (groundwork for "restore deleted items" feature)
        if (File.Exists(path))
        {
            File.Move(path, deletePath);
            return;
        }

        logger.LogError($"Could not delete image {guid} as it does not exist");
    }
}