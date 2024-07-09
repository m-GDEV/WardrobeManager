namespace WardrobeManager.Api.Database.Services.Interfaces;

public interface IFileService
{
    string GetDefaultUploadPath();
    Task<byte[]> GetImage(string guid);
    string ParseGuid(Guid guid);
    Task SaveImage(Guid? guid, string ImageBase64);
}