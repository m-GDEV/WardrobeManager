using WardrobeManager.Api.Services.Interfaces;

namespace WardrobeManager.Api.Services.Implementation;

public class FileService(
    IDataDirectoryService dataDirectoryService,
    IWebHostEnvironment webHostEnvironment,
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
        var max_file_size = configuration["WM_MAX_IMAGE_UPLOAD_SIZE_IN_MB"] ?? "5";
        int max_file_size_num = Convert.ToInt32(max_file_size);
        max_file_size_num *= 1024;

        if (imageBytes.Length > max_file_size_num)
        {
            throw new Exception(
                $"File size too large! Received file size: {imageBytes.Length / 1024} MB. Max file size: {max_file_size_num / 1024} MB");
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
}